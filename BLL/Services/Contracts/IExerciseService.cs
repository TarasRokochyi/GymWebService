using BLL.DTO;
using DAL.Models;

namespace BLL.Services.Contracts;

public interface IExerciseService
{
    Task<IEnumerable<ExerciseResponseDTO>> GetAllExercisesAsync();
    Task<ExerciseResponseDTO> GetExerciseByIdAsync(int id);
    Task<ExerciseResponseDTO> GetUserExerciseByIdAsync(int userId, int id);
    Task<IEnumerable<ExerciseResponseDTO>> GetExercisesByUserIdAsync(int id);
    Task<ExerciseResponseDTO> AddExerciseAsync(ExerciseRequestDTO exercise);
    Task<ExerciseResponseDTO> UpdateDefaultExerciseAsync(int id, ExerciseRequestDTO exercise);
    Task<ExerciseResponseDTO> UpdateUserExerciseAsync(int userId, int id, ExerciseRequestDTO exercise);
    Task DeleteUserExerciseAsync(int userId, int id);
    Task DeleteDefaultExerciseAsync(int id);
}