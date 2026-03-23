using AutoMapper;
using WeekMap.DTOs;
using WeekMap.Repositories.ActivityTemplate;

namespace WeekMap.Services.ActivityTemplate
{
    public class ActivityTemplateService : IActivityTemplateService
    {
        private readonly IActivityTemplateRepository _repo;
        private readonly IMapper _mapper;

        public ActivityTemplateService(IActivityTemplateRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<ActivityTemplateDTO>> GetAllAsync(long userId)
        {
            var templates = await _repo.GetAllAsync(userId);
            return _mapper.Map<List<ActivityTemplateDTO>>(templates);
        }

        public async Task<long> CreateAsync(long userId, ActivityTemplateDTO dto)
        {
            dto.UserID = userId;

            var entity = _mapper.Map<Models.ActivityTemplate>(dto);
            entity.UserID = userId;

            _repo.Create(entity);

            await _repo.SaveChangesAsync();

            return dto.ActivityTemplateID;
        }

        public async Task<bool> UpdateAsync(long userId, long id, ActivityTemplateDTO dto)
        {
            var template = await _repo.GetByIdOwnedAsync(userId, id);
            if (template == null) return false;

            _mapper.Map(dto, template);
            template.UserID = userId;

            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long userId, long id)
        {
            var template = await _repo.GetByIdOwnedAsync(userId, id);
            if (template == null) return false;

            _repo.Delete(template);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}