using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeekMap.Data;
using WeekMap.DTOs;

namespace WeekMap.Services.ActivityTemplate
{
    public class ActivityTemplateService : IActivityTemplateService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ActivityTemplateService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ActivityTemplateDTO>> GetAllAsync(long userId)
        {
            var templates = await _context.ActivityTemplates
                .Where(a => a.UserID == userId)
                .ToListAsync();

            return _mapper.Map<List<ActivityTemplateDTO>>(templates);
        }

        public async Task<long> CreateAsync(long userId, ActivityTemplateDTO dto)
        {
            dto.UserID = userId;

            var entity = _mapper.Map<Models.ActivityTemplate>(dto);
            entity.UserID = userId;

            _context.ActivityTemplates.Add(entity);
            await _context.SaveChangesAsync();

            return entity.ActivityTemplateID;
        }

        public async Task<bool> UpdateAsync(long userId, long id, ActivityTemplateDTO dto)
        {
            var template = await _context.ActivityTemplates
                .FirstOrDefaultAsync(a => a.ActivityTemplateID == id && a.UserID == userId);

            if (template == null) return false;

            _mapper.Map(dto, template);
            template.UserID = userId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long userId, long id)
        {
            var template = await _context.ActivityTemplates
                .FirstOrDefaultAsync(a => a.ActivityTemplateID == id && a.UserID == userId);

            if (template == null) return false;

            _context.ActivityTemplates.Remove(template);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
