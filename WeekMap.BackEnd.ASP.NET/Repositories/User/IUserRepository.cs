namespace WeekMap.Repositories.User
{
    public interface IUserRepository
    {
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string normalizedEmail);
        Task<List<Models.User>> GetAllAsync();
        Task<Models.User?> GetByIdAsync(long userId);
        Task<Models.User?> GetByEmailAsync(string email);
        Task<Models.User?> GetByEmailConfirmationTokenAsync(long userId, string token);

        void Create(Models.User user);
        void Delete(Models.User user);
        void AddUserSettings(Models.UserSettings settings);
        void AddUserDefaultWeekMapSettings(Models.UserDefaultWeekMapSettings settings);
        Task SaveChangesAsync();
    }
}