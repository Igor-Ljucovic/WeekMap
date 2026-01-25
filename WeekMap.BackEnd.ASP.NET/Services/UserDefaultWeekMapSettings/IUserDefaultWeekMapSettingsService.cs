using WeekMap.DTOs;

namespace WeekMap.Services.UserDefaultWeekMapSettings
{
    public interface IUserDefaultWeekMapSettingsService
    {
        Task<UserDefaultWeekMapSettingsDTO?> GetByUserIdAsync(long userId);
        Task<bool> UpdateAsync(long userId, UserDefaultWeekMapSettingsDTO dto);
    }
}
