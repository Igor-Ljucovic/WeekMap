using WeekMap.Models;

namespace WeekMap.Repositories.ActivityCategory
{
    public interface IActivityCategoryRepository
    {
        Task<List<Models.ActivityCategory>> GetAllAsync(long userId);
        Task<Models.ActivityCategory?> GetByIdOwnedAsync(long userId, long activityCategoryId);
        void Create(Models.ActivityCategory entity);
        void Delete(Models.ActivityCategory entity);
        Task SaveChangesAsync();
    }
}

