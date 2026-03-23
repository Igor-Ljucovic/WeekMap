using AutoMapper;
using WeekMap.DTOs;
using WeekMap.Repositories.WeekMapActivity;
using Models = WeekMap.Models;

namespace WeekMap.Services.WeekMapActivity
{
    public class WeekMapActivityService : IWeekMapActivityService
    {
        private readonly IWeekMapActivityRepository _repo;
        private readonly IMapper _mapper;

        public WeekMapActivityService(IWeekMapActivityRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<WeekMapActivityDTO?> GetByIdAsync(long userId, long weekMapActivityId)
        {
            var activity = await _repo.GetByIdWithWeekMapAsync(weekMapActivityId);

            if (activity == null) return null;
            if (activity.WeekMap == null || activity.WeekMap.UserID != userId) return null;

            return _mapper.Map<WeekMapActivityDTO>(activity);
        }

        public async Task<long?> CreateAsync(long userId, WeekMapActivityDTO dto)
        {
            var templateExists = await _repo.ActivityTemplateExistsAsync(dto.ActivityTemplateID);
            if (!templateExists)
                return null;

            var weekMap = await _repo.GetWeekMapByIdAsync(dto.WeekMapID);
            if (weekMap == null || weekMap.UserID != userId)
                return null;

            var entity = _mapper.Map<Models.WeekMapActivity>(dto);
            entity.ActivityTemplateID = dto.ActivityTemplateID;
            entity.WeekMapID = dto.WeekMapID;

            _repo.Create(entity);

            await _repo.SaveChangesAsync();
            return dto.WeekMapID;
        }

        public async Task<bool> UpdateAsync(long userId, long id, WeekMapActivityDTO dto)
        {
            var activity = await _repo.GetByIdWithWeekMapAsync(id);

            var targetWeekMapId = dto.WeekMapID != 0 ? dto.WeekMapID : activity.WeekMapID;

            var weekMap = await _repo.GetWeekMapByIdAsync(targetWeekMapId);

            _mapper.Map(dto, activity);

            activity.WeekMapID = targetWeekMapId;

            activity.ActivityTemplateID = dto.ActivityTemplateID;

            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long userId, long id)
        {
            var activity = await _repo.GetByIdWithWeekMapAsync(id);

            _repo.Delete(activity);
            await _repo.SaveChangesAsync();

            return true;
        }
    }
}