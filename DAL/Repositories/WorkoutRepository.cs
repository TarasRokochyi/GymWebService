using DAL.Models;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class WorkoutRepository : GenericRepository<Workout>, IWorkoutRepository
{
    public WorkoutRepository(GymWebServiceContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Workout>> GetByUserId(int userId)
    {
        var result = await table.Where(t => t.UserId == userId).ToListAsync();
        return result;
    }
}