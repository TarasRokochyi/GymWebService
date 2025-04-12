namespace BLL.DTO;

public class WorkoutTemplateResponseDTO
{
    
    public int TemplateId { get; set; }

    public int? UserId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
}