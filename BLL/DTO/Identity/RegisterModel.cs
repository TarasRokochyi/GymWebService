using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Identity;

public class RegisterModel
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    
    public string? Level { get; set; }

    public string? Gender { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Height { get; set; }

    public int? Age { get; set; }
}