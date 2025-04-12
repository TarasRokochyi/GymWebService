using BLL.DTO;
using BLL.Services.Contracts;
using DAL.Models;
using DAL.UOW;
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

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponseDTO>> GetUser(int id)
    {
        var result = await _userService.GetUserByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<UserResponseDTO>> AddUser(UserRequestDTO user)
    {
        var result = await _userService.AddUserAsync(user);
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
    
}
