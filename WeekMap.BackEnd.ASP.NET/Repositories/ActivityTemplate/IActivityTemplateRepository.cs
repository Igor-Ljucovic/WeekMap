namespace WeekMap.Repositories.ActivityTemplate
{
    public interface IActivityTemplateRepository
    {
        Task<List<Models.ActivityTemplate>> GetAllAsync(long userId);
        Task<Models.ActivityTemplate?> GetByIdOwnedAsync(long userId, long activityTemplateId);
        void Create(Models.ActivityTemplate entity);
        void Delete(Models.ActivityTemplate entity);
    }
}