namespace BLL.DTO;

public class WorkoutResponseDTO
{
    public int WorkoutId { get; set; }
    
    public int UserId { get; set; }

    public string? Name { get; set; }

    public DateTime? Date { get; set; }

    public int? Duration { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<WorkoutExerciseResponseDTO> WorkoutExercises { get; set; } = new List<WorkoutExerciseResponseDTO>();
}