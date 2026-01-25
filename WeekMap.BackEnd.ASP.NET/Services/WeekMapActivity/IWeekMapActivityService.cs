using WeekMap.DTOs;

namespace WeekMap.Services.WeekMapActivity
{
    public interface IWeekMapActivityService
    {
        Task<WeekMapActivityDTO?> GetByIdAsync(long userId, long weekMapActivityId);
        Task<(bool ok, string? errorMessage, long? id)> CreateAsync(long userId, WeekMapActivityDTO dto);
        Task<(bool ok, string? errorMessage)> UpdateAsync(long userId, long id, WeekMapActivityDTO dto);
        Task<(bool ok, string? errorMessage)> DeleteAsync(long userId, long id);
    }
}
