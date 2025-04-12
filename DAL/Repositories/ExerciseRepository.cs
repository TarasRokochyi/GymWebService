using DAL.Models;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class ExerciseRepository : GenericRepository<Exercise>, IExerciseRepository
{
    public ExerciseRepository(GymWebServiceContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Exercise>> GetByUserId(int userId)
    {
        var result = await table.Where(e => e.UserId == userId).ToListAsync();
        return result;
    }
}