using WeekMap.Models;

namespace WeekMap.Repositories.WeekMapActivity
{
    public interface IWeekMapActivityRepository
    {
        Task<Models.WeekMapActivity?> GetByIdWithWeekMapAsync(long weekMapActivityId);
        Task<bool> ActivityTemplateExistsAsync(long activityTemplateId);
        Task<Models.WeekMap?> GetWeekMapByIdAsync(long weekMapId);
        void Create(Models.WeekMapActivity entity);
        void Delete(Models.WeekMapActivity entity);
        Task SaveChangesAsync();
    }
}