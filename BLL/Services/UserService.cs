using AutoMapper;
using BLL.DTO;
using BLL.Services.Contracts;
using DAL.Models;
using DAL.UOW;

namespace BLL.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    

    public UserService(
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
    {
        var users = await _unitOfWork.UserRepository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<UserResponseDTO>>(users);
        return result;
    }

    public async Task<UserResponseDTO> GetUserByIdAsync(int userId)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        var result = _mapper.Map<UserResponseDTO>(user);
        return result;
    }

    public async Task<UserResponseDTO> AddUserAsync(UserRequestDTO user)
    {
        var userToAdd = _mapper.Map<User>(user);
        var addedUser = await _unitOfWork.UserRepository.AddAsync(userToAdd);
        await _unitOfWork.CompleteAsync();
        var result = _mapper.Map<UserResponseDTO>(addedUser);
        return result;
    }

    public async Task<UserResponseDTO> UpdateUserAsync(int id, UserRequestDTO user)
    {
        var userToUpdate = _mapper.Map<User>(user);
        userToUpdate.UserId = id;
        var updatedUser = await _unitOfWork.UserRepository.UpdateAsync(userToUpdate);
        await _unitOfWork.CompleteAsync();
        var result = _mapper.Map<UserResponseDTO>(updatedUser);
        return result;
    }

    public async Task DeleteUserAsync(int id)
    {
        await _unitOfWork.UserRepository.DeleteByIdAsync(id);
        await _unitOfWork.CompleteAsync();
    }
}