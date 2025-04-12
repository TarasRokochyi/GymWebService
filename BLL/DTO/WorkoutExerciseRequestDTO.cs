namespace BLL.DTO;

public class WorkoutExerciseRequestDTO
{
    public int? WorkoutId { get; set; }

    public int? ExerciseId { get; set; }

    public string? Type { get; set; }

    public int? Sets { get; set; }

    public int? Reps { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Distance { get; set; }

    public int? Duration { get; set; }

    public string? Notes { get; set; }
}