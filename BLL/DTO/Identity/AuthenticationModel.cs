using System.Text.Json.Serialization;

namespace BLL.DTO.Identity;

public class AuthenticationModel
{
    public string Message { get; set; }
    public bool IsAuthenticated { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
    public string Token { get; set; } 
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}