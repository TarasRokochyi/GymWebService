using BLL.DTO.Identity;
using DAL.Models;

namespace BLL.DTO;

public class UserResponseDTO
{
    
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? Level { get; set; }

    public string? Gender { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Height { get; set; }

    public int? Age { get; set; }

    public DateTime? CreatedAt { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }
}