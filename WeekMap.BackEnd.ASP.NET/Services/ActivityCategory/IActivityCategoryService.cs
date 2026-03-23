using WeekMap.DTOs;

namespace WeekMap.Services.ActivityCategory
{
    public interface IActivityCategoryService
    {
        Task<List<ActivityCategoryDTO>> GetAllAsync(long userId);
        Task<long> CreateAsync(long userId, ActivityCategoryDTO dto);
        Task<bool> UpdateAsync(long userId, long id, ActivityCategoryDTO dto);
        Task<bool> DeleteAsync(long userId, long id);
    }
}


