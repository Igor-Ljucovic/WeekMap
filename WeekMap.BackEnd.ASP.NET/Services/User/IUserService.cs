using WeekMap.DTOs;

namespace WeekMap.Services.User
{
    public interface IUserService
    {
        Task<(bool ok, string message, long? userId)> RegisterAsync(UserDTO dto);
        Task<(bool ok, string message, string? accessToken, long? userId, string? username)> LoginAsync(LoginDTO dto);
        Task<(bool ok, string message)> ConfirmEmailAsync(string token, long userId);

        Task<List<object>> GetAllAsync();
        Task<object?> GetByIdAsync(long id);
        Task<(bool ok, string message, long? userId)> CreateAsync(UserDTO dto);
        Task<(bool ok, string message)> UpdateAsync(long id, UserDTO dto);
        Task<(bool ok, string message)> DeleteAsync(long id);
    }
}
