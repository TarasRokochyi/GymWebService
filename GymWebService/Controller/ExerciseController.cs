using BLL.DTO;
using BLL.Services.Contracts;
using DAL.Models;
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExerciseResponseDTO>>> GetExercisesAsync()
    {
        var result = await _exerciseService.GetAllExercisesAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExerciseResponseDTO>> GetExerciseAsync(int id)
    {
        var result = await _exerciseService.GetExerciseByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ExerciseResponseDTO>> PostExerciseAsync(ExerciseRequestDTO exercise)
    {
        var result = await _exerciseService.AddExerciseAsync(exercise);
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<ExerciseResponseDTO>> PutExerciseAsync(int id, ExerciseRequestDTO exercise)
    {
        var result = await _exerciseService.UpdateExerciseAsync(id, exercise);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteExerciseAsync(int id)
    {
        await _exerciseService.DeleteExerciseAsync(id);
        return NoContent();
    }
}