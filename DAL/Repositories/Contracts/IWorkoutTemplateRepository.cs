using DAL.Models;

namespace DAL.Repositories.Contracts;

public interface IWorkoutTemplateRepository : IGenericRepository<WorkoutTemplate>
{
    Task<IEnumerable<WorkoutTemplate>> GetByUserId(int userId);
}