using WeekMap.DTOs;

namespace WeekMap.Services.WeekMapActivity
{
    public interface IWeekMapActivityService
    {
        Task<WeekMapActivityDTO?> GetByIdAsync(long userId, long weekMapActivityId);
        Task<long?> CreateAsync(long userId, WeekMapActivityDTO dto);
        Task<bool> UpdateAsync(long userId, long id, WeekMapActivityDTO dto);
        Task<bool> DeleteAsync(long userId, long id);
    }
}
