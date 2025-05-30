using AutoMapper;
using BLL.DTO;
using BLL.Services.Contracts;
using DAL.Models;
using DAL.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BLL.DTO.Identity;
using DAL.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Services;

public class UserService : IUserService
{
    private readonly GymWebServiceContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly JWT _jwt;

    public UserService(
        GymWebServiceContext context,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        UserManager<User> userManager,
        IOptions<JWT> jwt,
        RoleManager<IdentityRole<int>> roleManager
    )
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwt = jwt.Value;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
    {
        //var users = await _unitOfWork.UserRepository.GetAllAsync();
        var users = await _userManager.Users.ToListAsync();
        var result = _mapper.Map<IEnumerable<UserResponseDTO>>(users);
        return result;
    }

    public async Task<UserResponseDTO> UpdateUserAsync(int id, UserRequestDTO user)
    {
        var user1 = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        var userToUpdate = _mapper.Map(user, user1);
        //userToUpdate.Id = id;
        var identityResult = await _userManager.UpdateAsync(userToUpdate);
        await _unitOfWork.CompleteAsync();
        var updatedUser = await _userManager.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        var result = _mapper.Map<UserResponseDTO>(updatedUser);
        return result;
    }
    
    public async Task<string> UpdatePasswordAsync(int id, UpdatePasswordRequest request)
    {
        var user = await _userManager.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

        if (user == null)
            return "User not found.";
        try
        {
            await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            await _unitOfWork.CompleteAsync();

            return "Password updated successfully.";
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred when changing password: {ex.Message}");
        }
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userManager.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        _userManager.DeleteAsync(user);
        await _unitOfWork.CompleteAsync();
    }
    
    public async Task<string> RegisterAsync(RegisterModel model)
    {
        var user = new User
        {
            UserName = model.Username,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };
        var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
        if (userWithSameEmail == null)
        {
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, AuthorizationConst.default_role.ToString());

                return $"User Registered with username {user.UserName}";
            }
            throw new Exception(result.ToString());
        }
        else
        {
            throw new Exception($"Email {user.Email } is already registered.");
        }
    }
    
    public async Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model)
    {
        var authenticationModel = new AuthenticationModel();
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = $"No Accounts Registered with {model.Email}.";
            return authenticationModel;
        }
        if (await _userManager.CheckPasswordAsync(user, model.Password))
        {
            authenticationModel.IsAuthenticated = true;
            JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
            authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authenticationModel.Email = user.Email;
            authenticationModel.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            authenticationModel.Roles = rolesList.ToList();
            
            if (user.RefreshTokens.Any(a => a.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
                authenticationModel.RefreshToken = activeRefreshToken.Token;
                authenticationModel.RefreshTokenExpiration = activeRefreshToken.Expires;
            }
            else
            {
                var refreshToken = CreateRefreshToken();
                authenticationModel.RefreshToken = refreshToken.Token;
                authenticationModel.RefreshTokenExpiration = refreshToken.Expires;
                user.RefreshTokens.Add(refreshToken);
                _context.Update(user);
                _context.SaveChanges();
            }
            
            return authenticationModel;
        }
        authenticationModel.IsAuthenticated = false;
        authenticationModel.Message = $"Incorrect Credentials for user {user.Email}.";
        return authenticationModel;
    }
    
    public async Task<string> AddRoleAsync(AddRoleModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return $"No Accounts Registered with {model.Email}.";
        }
        if (await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var roleExists = Enum.GetNames(typeof(AuthorizationConst.Roles)).Any(x => x.ToLower() == model.Role.ToLower());
            if (roleExists)
            {
                var validRole = Enum.GetValues(typeof(AuthorizationConst.Roles)).Cast<AuthorizationConst.Roles>().Where(x => x.ToString().ToLower() == model.Role.ToLower()).FirstOrDefault();
                await _userManager.AddToRoleAsync(user, validRole.ToString());
                return $"Added {model.Role} to user {model.Email}.";
            }
            return $"Role {model.Role} not found.";
        }
        return $"Incorrect Credentials for user {user.Email}.";
    }
    
    public async Task<AuthenticationModel> RefreshTokenAsync(string token)
    {
        var authenticationModel = new AuthenticationModel();
        var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
        if (user == null)
        {
            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = $"Token did not match any users.";
            return authenticationModel;
        }

        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (!refreshToken.IsActive)
        {
            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = $"Token Not Active.";
            return authenticationModel;
        }

        //Revoke Current Refresh Token
        refreshToken.Revoked = DateTime.UtcNow;

        //Generate new Refresh Token and save to Database
        var newRefreshToken = CreateRefreshToken();
        user.RefreshTokens.Add(newRefreshToken);
        _context.Update(user);
        _context.SaveChanges();

        //Generates new jwt
        authenticationModel.IsAuthenticated = true;
        JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
        authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        authenticationModel.Email = user.Email;
        authenticationModel.UserName = user.UserName;
        var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        authenticationModel.Roles = rolesList.ToList();
        authenticationModel.RefreshToken = newRefreshToken.Token;
        authenticationModel.RefreshTokenExpiration = newRefreshToken.Expires;
        return authenticationModel;
    }

    public async Task<UserResponseDTO> GetByIdAsync(int id)
    {
        var user = await _userManager.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        var result = _mapper.Map<UserResponseDTO>(user);
        return result;
    }
    
    public bool RevokeToken(string token)
    {
        var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

        // return false if no user found with token
        if (user == null) return false;

        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        // return false if token is not active
        if (!refreshToken.IsActive) return false;

        // revoke token and save
        refreshToken.Revoked = DateTime.UtcNow;
        _context.Update(user);
        _context.SaveChanges();

        return true;
    }
    
    private async Task<JwtSecurityToken> CreateJwtToken(User user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();

        for (int i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim("roles", roles[i]));
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id.ToString())
        }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    } 
    
    private RefreshToken CreateRefreshToken()
    {
        var randomNumber = new byte[32];
        using(var generator = new RNGCryptoServiceProvider())
        {
            generator.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                Expires = DateTime.UtcNow.AddDays(10),
                Created = DateTime.UtcNow
            };
        }
    }
}