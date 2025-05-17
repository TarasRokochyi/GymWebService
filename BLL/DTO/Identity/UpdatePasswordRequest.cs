using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Identity;

public class UpdatePasswordRequest
{
    [Required]
    public string CurrentPassword { get; set; }
    [Required]
    public string NewPassword { get; set; }
}