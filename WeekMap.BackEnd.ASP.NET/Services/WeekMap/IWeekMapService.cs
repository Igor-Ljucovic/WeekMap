using WeekMap.DTOs;

namespace WeekMap.Services.WeekMap
{
    public interface IWeekMapService
    {
        Task<List<Models.WeekMap>> GetAllAsync(long userId);

        // Your controller's GetById actually means: "latest map for userId"
        Task<Models.WeekMap?> GetLatestByUserIdAsync(long sessionUserId, long requestedUserId);

        // Returns activities for a WeekMap (with ActivityTemplate + ActivityCategory included)
        Task<List<Models.WeekMapActivity>> GetWeekMapActivitiesAsync(long sessionUserId, long weekMapId);

        Task<long> CreateAsync(long userId, WeekMapDTO dto);
        Task<bool> UpdateAsync(long userId, long id, WeekMapDTO dto);
        Task<bool> DeleteAsync(long userId, long id);
    }
}
