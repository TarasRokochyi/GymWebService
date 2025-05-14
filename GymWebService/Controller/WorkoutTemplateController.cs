using BLL.DTO;
using BLL.Services.Contracts;
using DAL.Repositories.Contracts;
using GymWebService.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GymWebService.Controller;

[ApiController]

[Route("/api/[controller]")]
public class WorkoutTemplateController : ControllerBase
{
    
    private readonly ILogger<WorkoutTemplateController> _logger;
    
    private IWorkoutTemplateService _workoutTemplateService;

    public WorkoutTemplateController(IWorkoutTemplateService workoutTemplateService, ILogger<WorkoutTemplateController> logger)
    {
        _workoutTemplateService = workoutTemplateService;
        _logger = logger;
    }
    
    [HttpPost("addDefault")]
    public async Task<ActionResult> PostDefaultWorkoutTemplate(WorkoutTemplateRequestDTO workoutTemplateRequest)
    {
        workoutTemplateRequest.UserId = null;
        var result = await _workoutTemplateService.AddWorkoutTemplateAsync(workoutTemplateRequest);
        return Ok(result);
    }
    
    
    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<WorkoutTemplateResponseDTO>>> GetAllWorkoutTemplates()
    {
        var result = await _workoutTemplateService.GetAllWorkoutTemplatesAsync();
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkoutTemplateResponseDTO>>> GetUserWorkoutTemplate()
    {
        int userId = HttpContext.GetUserId();
        var result = await _workoutTemplateService.GetAllWorkoutTemplatesByUserIdAsync(userId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkoutTemplateResponseDTO>> GetWorkoutTemplates(int id)
    {
        var result = await _workoutTemplateService.GetWorkoutTemplateByIdAsync(id);
        return Ok(result);
    }

    
    [HttpPost]
    public async Task<ActionResult> PostWorkoutTemplate(WorkoutTemplateRequestDTO workoutTemplateRequest)
    {
        workoutTemplateRequest.UserId = HttpContext.GetUserId();
        var result = await _workoutTemplateService.AddWorkoutTemplateAsync(workoutTemplateRequest);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutWorkoutTemplate(int id, WorkoutTemplateRequestDTO workoutTemplateRequest)
    {
        var result = await _workoutTemplateService.UpdateWorkoutTemplateAsync(id, workoutTemplateRequest);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteWorkoutTemplate(int id)
    {
        await _workoutTemplateService.DeleteWorkoutTemplateAsync(id);
        return NoContent();
    }
    
}