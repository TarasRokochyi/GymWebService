using DAL.Models;

namespace DAL.Repositories.Contracts;

public interface IWorkoutRepository : IGenericRepository<Workout>
{
    Task<IEnumerable<Workout>> GetByUserIdAsync(int userId);
    Task<Workout> GetUserWorkoutAsync(int userId, int id);
    
}
