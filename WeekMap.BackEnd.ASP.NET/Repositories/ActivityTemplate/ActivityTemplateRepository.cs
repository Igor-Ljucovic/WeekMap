using Microsoft.EntityFrameworkCore;
using WeekMap.Data;

namespace WeekMap.Repositories.ActivityTemplate
{
    public class ActivityTemplateRepository : IActivityTemplateRepository
    {
        private readonly AppDbContext _context;

        public ActivityTemplateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Models.ActivityTemplate>> GetAllAsync(long userId)
        {
            return await _context.ActivityTemplates
                .Where(a => a.UserID == userId)
                .ToListAsync();
        }

        public async Task<Models.ActivityTemplate?> GetByIdOwnedAsync(long userId, long activityTemplateId)
        {
            return await _context.ActivityTemplates
                .FirstOrDefaultAsync(a => a.ActivityTemplateID == activityTemplateId && a.UserID == userId);
        }

        public void Create(Models.ActivityTemplate entity)
        {
            _context.ActivityTemplates.Add(entity);
        }

        public void Delete(Models.ActivityTemplate entity)
        {
            _context.ActivityTemplates.Remove(entity);
        }
    }
}