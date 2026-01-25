using WeekMap.DTOs;

namespace WeekMap.Services.UserSettings
{
    public interface IUserSettingsService
    {
        Task<UserSettingsDTO?> GetByUserIdAsync(long userId);
        Task<bool> UpdateAsync(long userId, UserSettingsDTO dto);
    }
}
