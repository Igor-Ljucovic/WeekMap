using Microsoft.EntityFrameworkCore;
using WeekMap.Data;

namespace WeekMap.Repositories.WeekMapActivity
{
    public class WeekMapActivityRepository : IWeekMapActivityRepository
    {
        private readonly AppDbContext _context;

        public WeekMapActivityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Models.WeekMapActivity?> GetByIdWithWeekMapAsync(long weekMapActivityId)
        {
            return await _context.WeekMapActivities
                .Include(a => a.WeekMap)
                .FirstOrDefaultAsync(a => a.WeekMapActivityID == weekMapActivityId);
        }

        public async Task<bool> ActivityTemplateExistsAsync(long activityTemplateId)
        {
            return await _context.ActivityTemplates
                .AnyAsync(a => a.ActivityTemplateID == activityTemplateId);
        }

        public async Task<Models.WeekMap?> GetWeekMapByIdAsync(long weekMapId)
        {
            return await _context.WeekMaps
                .FirstOrDefaultAsync(wm => wm.WeekMapID == weekMapId);
        }

        public void Create(Models.WeekMapActivity entity)
        {
            _context.WeekMapActivities.Add(entity);
        }

        public void Delete(Models.WeekMapActivity entity)
        {
            _context.WeekMapActivities.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}