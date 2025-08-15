using TodoApi.DTOs;

namespace TodoApi.Services
{
    public interface IUserService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginUserDto loginDto);
        Task<UserResponseDto?> GetUserByIdAsync(int id);
        Task<bool> UserExistsAsync(string username, string email);
    }
}
