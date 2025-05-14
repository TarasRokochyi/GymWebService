using BLL.DTO;
using BLL.Services.Contracts;
using DAL.Models;
using GymWebService.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GymWebService.Controller;

[ApiController]

[Route("/api/[controller]")]

public class ExerciseController : ControllerBase
{
    private readonly ILogger<WorkoutController> _logger;
    
    private IExerciseService _exerciseService;
 public ExerciseController(IExerciseService exerciseService, ILogger<WorkoutController> logger)
    {
        _exerciseService = exerciseService;
        _logger = logger;
    }
    
    [HttpPost("addDefault")]
    public async Task<ActionResult<ExerciseResponseDTO>> PostDefaultExercise(ExerciseRequestDTO exercise)
    {
        exercise.UserId = null;
        var result = await _exerciseService.AddExerciseAsync(exercise);
        return Ok(result);
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<ExerciseResponseDTO>>> GetAllExercises()
    {
        var result = await _exerciseService.GetAllExercisesAsync();
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExerciseResponseDTO>>> GetUserExercises()
    {
        int userId = HttpContext.GetUserId();
        var result = await _exerciseService.GetExercisesByUserIdAsync(userId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExerciseResponseDTO>> GetExercise(int id)
    {
        var result = await _exerciseService.GetExerciseByIdAsync(id);
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
    public async Task<ActionResult<ExerciseResponseDTO>> PutExercise(int id, ExerciseRequestDTO exercise)
    {
        var result = await _exerciseService.UpdateExerciseAsync(id, exercise);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteExercise(int id)
    {
        await _exerciseService.DeleteExerciseAsync(id);
        return NoContent();
    }
}