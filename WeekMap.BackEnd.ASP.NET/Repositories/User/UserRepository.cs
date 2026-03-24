using Microsoft.EntityFrameworkCore;
using WeekMap.Data;

namespace WeekMap.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> EmailExistsAsync(string normalizedEmail)
        {
            return await _context.Users.AnyAsync(u => u.Email.ToLower() == normalizedEmail);
        }

        public async Task<List<Models.User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Models.User?> GetByIdAsync(long userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
        }

        public async Task<Models.User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Models.User?> GetByEmailConfirmationTokenAsync(long userId, string token)
        {
            return await _context.Users.FirstOrDefaultAsync(u =>
                u.UserID == userId &&
                u.EmailConfirmationToken == token);
        }

        public void Create(Models.User user)
        {
            _context.Users.Add(user);
            // intentionally not saving here, so that we can save the user and all the settings as 1 transaction/unit of work
            // await _context.SaveChangesAsync();
        }

        public void AddUserSettings(Models.UserSettings settings)
        {
            _context.UserSettings.Add(settings);
        }

        public void AddUserDefaultWeekMapSettings(Models.UserDefaultWeekMapSettings settings)
        {
            _context.UserDefaultWeekMapSettings.Add(settings);
        }

        public void Delete(Models.User user)
        {
            _context.Users.Remove(user);
        }
    }
}