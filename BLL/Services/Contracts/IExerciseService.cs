using BLL.DTO;
using DAL.Models;

namespace BLL.Services.Contracts;

public interface IExerciseService
{
    Task<IEnumerable<ExerciseResponseDTO>> GetAllExercisesAsync();
    Task<ExerciseResponseDTO> GetExerciseByIdAsync(int id);
    Task<IEnumerable<ExerciseResponseDTO>> GetExercisesByUserIdAsync(int id);
    Task<ExerciseResponseDTO> AddExerciseAsync(ExerciseRequestDTO exercise);
    Task<ExerciseResponseDTO> UpdateExerciseAsync(int id, ExerciseRequestDTO exercise);
    Task DeleteExerciseAsync(int id);
}