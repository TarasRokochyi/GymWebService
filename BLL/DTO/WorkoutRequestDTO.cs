namespace BLL.DTO;

public class WorkoutRequestDTO
{
    public int? UserId { get; set; }

    public string? Name { get; set; }

    public DateTime? Date { get; set; }

    public int? Duration { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<WorkoutExerciseRequestDTO> WorkoutExercises { get; set; } = new List<WorkoutExerciseRequestDTO>();
}