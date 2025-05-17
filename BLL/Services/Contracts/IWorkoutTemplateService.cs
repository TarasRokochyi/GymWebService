using BLL.DTO;

namespace BLL.Services.Contracts;

public interface IWorkoutTemplateService
{
    Task<IEnumerable<WorkoutTemplateResponseDTO>> GetAllWorkoutTemplatesAsync();
    Task<WorkoutTemplateResponseDTO> GetWorkoutTemplateByIdAsync(int id);
    Task<WorkoutTemplateResponseDTO> GetUserWorkoutTemplateByIdAsync(int userId, int id);
    Task<IEnumerable<WorkoutTemplateResponseDTO>> GetAllWorkoutTemplatesByUserIdAsync(int userId);
    Task<WorkoutTemplateResponseDTO> AddWorkoutTemplateAsync(WorkoutTemplateRequestDTO template);
    Task<WorkoutTemplateResponseDTO> UpdateUserWorkoutTemplateAsync(int userId, int id, WorkoutTemplateRequestDTO template);
    Task<WorkoutTemplateResponseDTO> UpdateDefaultWorkoutTemplateAsync(int id, WorkoutTemplateRequestDTO template);
    Task DeleteUserWorkoutTemplateAsync(int userId, int id);
    Task DeleteDefaultWorkoutTemplateAsync(int id);
}