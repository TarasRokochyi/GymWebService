using BLL.DTO.Identity;

namespace BLL.DTO;

public class UserResponseDTO
{
    
    public int UserId { get; set; }

    public string? Name { get; set; }

    public string? Level { get; set; }

    public string? Gender { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Height { get; set; }

    public int? Age { get; set; }

    public DateTime? CreatedAt { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }
}