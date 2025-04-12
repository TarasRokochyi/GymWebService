using BLL.DTO;

namespace BLL.Services.Contracts;

public interface IWorkoutService
{
    
    Task<IEnumerable<WorkoutResponseDTO>> GetAllWorkoutsAsync();
    Task<IEnumerable<WorkoutResponseDTO>> GetAllWorkoutsByUserIdAsync(int userId);
    Task<WorkoutResponseDTO> GetWorkoutByIdAsync(int id);
    Task<WorkoutResponseDTO> AddWorkoutAsync(WorkoutRequestDTO workout);
    Task<WorkoutResponseDTO> UpdateWorkoutAsync(int id, WorkoutRequestDTO workout);
    Task DeleteWorkoutAsync(int id);
}