using DAL.Models;

namespace DAL.Repositories.Contracts;

public interface IWorkoutRepository : IGenericRepository<Workout>
{
    Task<IEnumerable<Workout>> GetByUserId(int userId);
    
}
