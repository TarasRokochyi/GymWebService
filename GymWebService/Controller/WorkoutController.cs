using BLL.DTO;
using BLL.Services.Contracts;
using GymWebService.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GymWebService.Controller;

[ApiController]

[Route("/api/[controller]")]
public class WorkoutController : ControllerBase
{
    private readonly ILogger<WorkoutController> _logger;
    
    private IWorkoutService _workoutService;

    public WorkoutController(IWorkoutService workoutService, ILogger<WorkoutController> logger)
    {
        _workoutService = workoutService;
        _logger = logger;
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<WorkoutResponseDTO>>> GetAllWorkouts()
    {
        var result = await _workoutService.GetAllWorkoutsAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkoutResponseDTO>> GetWorkout(int id)
    {
        var result = await _workoutService.GetWorkoutByIdAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkoutResponseDTO>>> GetAllUserWorkouts()
    {
        int userId = HttpContext.GetUserId();
        var result = await _workoutService.GetAllWorkoutsByUserIdAsync(userId);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult> PostWorkout(WorkoutRequestDTO workoutRequest){
        int userId = HttpContext.GetUserId();
        workoutRequest.UserId = userId;
        var result = await _workoutService.AddWorkoutAsync(workoutRequest);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutWorkout(int id, WorkoutRequestDTO workoutRequest)
    {
        var result = await _workoutService.UpdateWorkoutAsync(id, workoutRequest);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteWorkout(int id)
    {
        await _workoutService.DeleteWorkoutAsync(id);
        return NoContent();
    }
}