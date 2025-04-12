namespace DAL.Models;

public class WorkoutExercise
{
    public int WorkoutExerciseId { get; set; }

    public int? WorkoutId { get; set; }

    public int? ExerciseId { get; set; }

    public string? Type { get; set; }

    public int? Sets { get; set; }

    public int? Reps { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Distance { get; set; }

    public int? Duration { get; set; }

    public string? Notes { get; set; }

    public virtual Exercise? Exercise { get; set; }

    public virtual Workout? Workout { get; set; }

}