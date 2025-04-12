using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.Contracts;

namespace DAL.UOW;

public class UnitOfWork : IUnitOfWork
{
    private readonly GymWebServiceContext _context;
    public IExerciseRepository ExerciseRepository { get; }
    public IUserRepository UserRepository { get; }
    public IWorkoutExerciseRepository WorkoutExerciseRepository { get; }
    public IWorkoutRepository WorkoutRepository { get; }
    public IWorkoutTemplateRepository WorkoutTemplateRepository { get; }

    public UnitOfWork(GymWebServiceContext context,
        IUserRepository userRepository,
        IExerciseRepository exerciseRepository,
        IWorkoutExerciseRepository workoutExerciseRepository,
        IWorkoutRepository workoutRepository,
        IWorkoutTemplateRepository workoutTemplateRepository)
    {
        _context = context;
        ExerciseRepository = exerciseRepository;
        UserRepository = userRepository;
        WorkoutExerciseRepository = workoutExerciseRepository;
        WorkoutRepository = workoutRepository;
        WorkoutTemplateRepository = workoutTemplateRepository;
    }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}