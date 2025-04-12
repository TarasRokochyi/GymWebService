namespace DAL.Models;

public class WorkoutTemplate
{
    public int TemplateId { get; set; }

    public int? UserId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual User? User { get; set; }
}