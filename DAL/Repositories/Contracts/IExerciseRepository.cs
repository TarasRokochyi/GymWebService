using DAL.Models;

namespace DAL.Repositories.Contracts;

public interface IExerciseRepository : IGenericRepository<Exercise>
{
    Task<IEnumerable<Exercise>> GetByUserId(int userId);
}