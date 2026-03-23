using WeekMap.DTOs;

namespace WeekMap.Services.User
{
    public interface IUserService
    {
        Task<(bool ok, long? userId)> RegisterAsync(UserDTO dto);
        Task<(bool ok, string? accessToken, long? userId, string? username)> LoginAsync(LoginDTO dto);
        Task<bool> ConfirmEmailAsync(string token, long userId);

        Task<List<object>> GetAllAsync();
        Task<object?> GetByIdAsync(long id);
        Task<bool> UpdateAsync(long id, UserDTO dto);
        Task<bool> DeleteAsync(long id);
    }
}
