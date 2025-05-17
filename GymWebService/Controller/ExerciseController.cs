using BLL.DTO;
using BLL.Services.Contracts;
using DAL.Models;
using GymWebService.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymWebService.Controller;

[ApiController]
[Route("/api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ExerciseController : ControllerBase
{
    private readonly ILogger<WorkoutController> _logger;
    
    private IExerciseService _exerciseService;
 public ExerciseController(IExerciseService exerciseService, ILogger<WorkoutController> logger)
    {
        _exerciseService = exerciseService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExerciseResponseDTO>>> GetUserExercises()
    {
        int userId = HttpContext.GetUserId();
        var result = await _exerciseService.GetExercisesByUserIdAsync(userId);
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ExerciseResponseDTO>> GetUserExercise(int id)
    {
        var userId = HttpContext.GetUserId();
        var result = await _exerciseService.GetUserExerciseByIdAsync(userId, id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ExerciseResponseDTO>> PostExercise(ExerciseRequestDTO exercise)
    {
        exercise.UserId = HttpContext.GetUserId();
        var result = await _exerciseService.AddExerciseAsync(exercise);
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<ExerciseResponseDTO>> PutUserExercise(int id, ExerciseRequestDTO exercise)
    {
        var userId = HttpContext.GetUserId();
        var result = await _exerciseService.UpdateUserExerciseAsync(userId, id, exercise);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUserExercise(int id)
    {
        int userId = HttpContext.GetUserId();
        await _exerciseService.DeleteUserExerciseAsync(userId, id);
        return NoContent();
    }
}