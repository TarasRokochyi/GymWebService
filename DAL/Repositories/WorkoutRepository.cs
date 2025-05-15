using DAL.Models;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class WorkoutRepository : GenericRepository<Workout>, IWorkoutRepository
{
    public WorkoutRepository(GymWebServiceContext context) : base(context)
    {
    }

    public override async Task<Workout> GetByIdAsync(int id)
    {
        var result = await table.Where(w => w.WorkoutId == id).Include(w => w.WorkoutExercises).ThenInclude(w => w.Exercise).FirstOrDefaultAsync();
        return result;
    }

    public async Task<IEnumerable<Workout>> GetByUserId(int userId)
    {
        var result = await table.Where(t => t.UserId == userId).ToListAsync();
        return result;
    }
}