using DAL.Models;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class WorkoutTemplateRepository : GenericRepository<WorkoutTemplate>, IWorkoutTemplateRepository
{
    public WorkoutTemplateRepository(GymWebServiceContext context) : base(context)
    {
    }

    public async Task<IEnumerable<WorkoutTemplate>> GetByUserId(int userId)
    {
        var result = await table.Where(t => t.UserId == userId || t.UserId == null).ToListAsync();
        return result;
    }
}