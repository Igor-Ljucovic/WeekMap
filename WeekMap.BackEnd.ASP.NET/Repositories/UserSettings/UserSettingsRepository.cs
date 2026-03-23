using Microsoft.EntityFrameworkCore;
using WeekMap.Data;

namespace WeekMap.Repositories.UserSettings
{
    public class UserSettingsRepository : IUserSettingsRepository
    {
        private readonly AppDbContext _context;

        public UserSettingsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Models.UserSettings?> GetByUserIdAsync(long userId)
        {
            return await _context.UserSettings
                .FirstOrDefaultAsync(us => us.UserID == userId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}