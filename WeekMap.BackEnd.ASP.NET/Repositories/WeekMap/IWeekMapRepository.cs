namespace WeekMap.Repositories.WeekMap
{
    public interface IWeekMapRepository
    {
        Task<List<Models.WeekMap>> GetAllAsync(long userId);
        Task<Models.WeekMap?> GetByIdOwnedAsync(long userId, long weekMapId);
        Task<Models.WeekMap?> GetLatestByUserIdAsync(long userId);
        Task<List<Models.WeekMapActivity>> GetWeekMapActivitiesAsync(long weekMapId);
        void Create(Models.WeekMap entity);
        void Delete(Models.WeekMap entity);
        Task SaveChangesAsync();
    }
}