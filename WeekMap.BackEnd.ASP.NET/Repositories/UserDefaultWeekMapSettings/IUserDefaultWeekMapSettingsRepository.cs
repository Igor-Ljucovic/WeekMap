namespace WeekMap.Repositories.UserDefaultWeekMapSettings
{
    public interface IUserDefaultWeekMapSettingsRepository
    {
        Task<Models.UserDefaultWeekMapSettings?> GetByUserIdAsync(long userId);
        Task SaveChangesAsync();
    }
}