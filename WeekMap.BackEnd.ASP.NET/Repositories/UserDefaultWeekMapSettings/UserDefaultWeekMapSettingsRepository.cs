using Microsoft.EntityFrameworkCore;
using WeekMap.Data;

namespace WeekMap.Repositories.UserDefaultWeekMapSettings
{
    public class UserDefaultWeekMapSettingsRepository : IUserDefaultWeekMapSettingsRepository
    {
        private readonly AppDbContext _context;

        public UserDefaultWeekMapSettingsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Models.UserDefaultWeekMapSettings?> GetByUserIdAsync(long userId)
        {
            return await _context.UserDefaultWeekMapSettings
                .FirstOrDefaultAsync(wms => wms.UserID == userId);
        }
    }
}