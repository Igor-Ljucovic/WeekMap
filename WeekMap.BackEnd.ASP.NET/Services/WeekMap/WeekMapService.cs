using AutoMapper;
using WeekMap.DTOs;
using WeekMap.Repositories.WeekMap;

namespace WeekMap.Services.WeekMap
{
    public class WeekMapService : IWeekMapService
    {
        private readonly IWeekMapRepository _repo;
        private readonly IMapper _mapper;

        public WeekMapService(IWeekMapRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<Models.WeekMap>> GetAllAsync(long userId)
        {
            return await _repo.GetAllAsync(userId);
        }

        public async Task<Models.WeekMap?> GetLatestByUserIdAsync(long sessionUserId, long requestedUserId)
        {
            if (sessionUserId != requestedUserId)
                return null;

            return await _repo.GetLatestByUserIdAsync(requestedUserId);
        }

        public async Task<List<Models.WeekMapActivity>> GetWeekMapActivitiesAsync(long sessionUserId, long weekMapId)
        {
            var ownsMap = await _repo.GetByIdOwnedAsync(sessionUserId, weekMapId);
            if (ownsMap == null)
                return new List<Models.WeekMapActivity>();

            return await _repo.GetWeekMapActivitiesAsync(weekMapId);
        }

        public async Task<long> CreateAsync(long userId, WeekMapDTO dto)
        {
            dto.UserID = userId;

            var entity = _mapper.Map<Models.WeekMap>(dto);
            entity.UserID = userId;

            _repo.Create(entity);

            await _repo.SaveChangesAsync();

            return dto.UserID;
        }

        public async Task<bool> UpdateAsync(long userId, long id, WeekMapDTO dto)
        {
            var map = await _repo.GetByIdOwnedAsync(userId, id);
            if (map == null) return false;

            _mapper.Map(dto, map);
            map.UserID = userId;

            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long userId, long id)
        {
            var map = await _repo.GetByIdOwnedAsync(userId, id);
            if (map == null) return false;

            _repo.Delete(map);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}