using BLL.DTO;
using DAL.Models;

namespace BLL.Services.Contracts;

public interface IUserService
{
    Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();
    Task<UserResponseDTO> GetUserByIdAsync(int userId);
    Task<UserResponseDTO> AddUserAsync(UserRequestDTO user);
    Task<UserResponseDTO> UpdateUserAsync(int id, UserRequestDTO user);
    Task DeleteUserAsync(int id);
}