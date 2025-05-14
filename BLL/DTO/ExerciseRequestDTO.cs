namespace BLL.DTO;

public class ExerciseRequestDTO
{
    public int? UserId { get; set; }
    
    public string? Name { get; set; }

    public string? Category { get; set; }

    public string? MuscleGroups { get; set; }

    public string? Description { get; set; }

    //public virtual ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
}