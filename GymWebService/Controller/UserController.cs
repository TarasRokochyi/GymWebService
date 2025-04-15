using BLL.DTO;
using BLL.DTO.Identity;
using BLL.Services.Contracts;
using DAL.Models;
using DAL.UOW;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymWebService.Controller;


[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    
    private IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetAllUsers()
    {
        var result = await _userService.GetAllUsersAsync();
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserResponseDTO>> UpdateUser(int id, UserRequestDTO user)
    {
        var result = await _userService.UpdateUserAsync(id, user);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
    
    
    
    
    
    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync(RegisterModel model)
    {
        var result = await _userService.RegisterAsync(model);
        return Ok(result);
    }
    
    [HttpPost("token")]
    public async Task<IActionResult> GetTokenAsync(TokenRequestModel model)
    {
        var result = await _userService.GetTokenAsync(model);
        SetRefreshTokenInCookie(result.RefreshToken);
        return Ok(result);
    }

    [HttpPost("addrole")]
    public async Task<IActionResult> AddRoleAsync(AddRoleModel model)
    {
        var result = await _userService.AddRoleAsync(model);
        return Ok(result);
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var response = await _userService.RefreshTokenAsync(refreshToken);
        if (!string.IsNullOrEmpty(response.RefreshToken))
            SetRefreshTokenInCookie(response.RefreshToken);
        return Ok(response);
    }

    [Authorize]
    [HttpPost("tokens/{id}")]
    public IActionResult GetRefreshTokens(int id)
    {
        var user = _userService.GetById(id);
        return Ok(user.RefreshTokens);
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
    {
        // accept token from request body or cookie
        var token = model.Token ?? Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(token))
            return BadRequest(new { message = "Token is required" });

        var response = _userService.RevokeToken(token);

        if (!response)
            return NotFound(new { message = "Token not found" });

        return Ok(new { message = "Token revoked" });
    }
    
    private void SetRefreshTokenInCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(10),
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
    
}
