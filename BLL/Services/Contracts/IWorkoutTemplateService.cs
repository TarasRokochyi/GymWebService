using BLL.DTO;

namespace BLL.Services.Contracts;

public interface IWorkoutTemplateService
{
    Task<IEnumerable<WorkoutTemplateResponseDTO>> GetAllWorkoutTemplatesAsync();
    Task<WorkoutTemplateResponseDTO> GetWorkoutTemplateByIdAsync(int id);
    Task<IEnumerable<WorkoutTemplateResponseDTO>> GetAllWorkoutTemplatesByUserIdAsync(int userId);
    Task<WorkoutTemplateResponseDTO> AddWorkoutTemplateAsync(WorkoutTemplateRequestDTO template);
    Task<WorkoutTemplateResponseDTO> UpdateWorkoutTemplateAsync(int id, WorkoutTemplateRequestDTO template);
    Task DeleteWorkoutTemplateAsync(int id);
}