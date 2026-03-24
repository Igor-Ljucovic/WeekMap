namespace WeekMap.Repositories.UserSettings
{
    public interface IUserSettingsRepository
    {
        Task<Models.UserSettings?> GetByUserIdAsync(long userId);
    }
}