using Microsoft.EntityFrameworkCore;
using WeekMap.Data;

namespace WeekMap.Repositories.ActivityCategory
{
    public class ActivityCategoryRepository : IActivityCategoryRepository
    {
        private readonly AppDbContext _context;

        public ActivityCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Models.ActivityCategory>> GetAllAsync(long userId)
        {
            return await _context.ActivityCategories
                .Where(c => c.UserID == userId)
                .ToListAsync();
        }

        public async Task<Models.ActivityCategory?> GetByIdOwnedAsync(long userId, long activityCategoryId)
        {
            return await _context.ActivityCategories
                .FirstOrDefaultAsync(c => c.ActivityCategoryID == activityCategoryId && c.UserID == userId);
        }

        public void Create(Models.ActivityCategory entity)
        {
            _context.ActivityCategories.Add(entity);
        }

        public void Delete(Models.ActivityCategory entity)
        {
            _context.ActivityCategories.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}