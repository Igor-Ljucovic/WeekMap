using Microsoft.EntityFrameworkCore;
using WeekMap.Data;

namespace WeekMap.Repositories.WeekMap
{
    public class WeekMapRepository : IWeekMapRepository
    {
        private readonly AppDbContext _context;

        public WeekMapRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Models.WeekMap>> GetAllAsync(long userId)
        {
            return await _context.WeekMaps
                .Where(wm => wm.UserID == userId)
                .ToListAsync();
        }

        public async Task<Models.WeekMap?> GetByIdOwnedAsync(long userId, long weekMapId)
        {
            return await _context.WeekMaps
                .FirstOrDefaultAsync(wm =>
                    wm.WeekMapID == weekMapId &&
                    wm.UserID == userId);
        }

        public async Task<Models.WeekMap?> GetLatestByUserIdAsync(long userId)
        {
            return await _context.WeekMaps
                .Where(wm => wm.UserID == userId)
                .OrderByDescending(wm => wm.DateCreated)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Models.WeekMapActivity>> GetWeekMapActivitiesAsync(long weekMapId)
        {
            return await _context.WeekMapActivities
                .Include(wma => wma.ActivityTemplate)
                .ThenInclude(at => at.ActivityCategory)
                .Where(wma => wma.WeekMapID == weekMapId)
                .ToListAsync();
        }

        public void Create(Models.WeekMap entity)
        {
            _context.WeekMaps.Add(entity);
        }

        public void Delete(Models.WeekMap entity)
        {
            _context.WeekMaps.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}