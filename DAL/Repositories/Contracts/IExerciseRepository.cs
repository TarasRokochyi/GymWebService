using DAL.Models;

namespace DAL.Repositories.Contracts;

public interface IExerciseRepository : IGenericRepository<Exercise>
{
    Task<IEnumerable<Exercise>> GetByUserIdAsync(int userId);
    Task<Exercise> GetUserExerciseAsync(int userId, int exerciseId);
}