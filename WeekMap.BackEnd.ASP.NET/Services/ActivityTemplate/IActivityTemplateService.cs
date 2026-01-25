using WeekMap.DTOs;

namespace WeekMap.Services.ActivityTemplate
{
    public interface IActivityTemplateService
    {
        Task<List<ActivityTemplateDTO>> GetAllAsync(long userId);
        Task<long> CreateAsync(long userId, ActivityTemplateDTO dto);
        Task<bool> UpdateAsync(long userId, long id, ActivityTemplateDTO dto);
        Task<bool> DeleteAsync(long userId, long id);
    }
}
