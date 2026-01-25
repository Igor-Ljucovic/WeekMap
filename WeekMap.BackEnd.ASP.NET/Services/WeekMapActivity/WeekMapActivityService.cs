using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeekMap.Data;
using WeekMap.DTOs;
using Models = WeekMap.Models;

namespace WeekMap.Services.WeekMapActivity
{
    public class WeekMapActivityService : IWeekMapActivityService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public WeekMapActivityService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<WeekMapActivityDTO?> GetByIdAsync(long userId, long weekMapActivityId)
        {
            var activity = await _context.WeekMapActivities
                .Include(a => a.WeekMap)
                .FirstOrDefaultAsync(a => a.WeekMapActivityID == weekMapActivityId);

            if (activity == null) return null;
            if (activity.WeekMap == null || activity.WeekMap.UserID != userId) return null;

            return _mapper.Map<WeekMapActivityDTO>(activity);
        }

        public async Task<(bool ok, string? errorMessage, long? id)> CreateAsync(long userId, WeekMapActivityDTO dto)
        {
            var templateExists = await _context.ActivityTemplates
                .AnyAsync(a => a.ActivityTemplateID == dto.ActivityTemplateID);

            if (!templateExists)
                return (false, $"ActivityTemplate with ID {dto.ActivityTemplateID} does not exist.", null);

            var weekMap = await _context.WeekMaps
                .FirstOrDefaultAsync(wm => wm.WeekMapID == dto.WeekMapID);

            if (weekMap == null)
                return (false, $"PlannedWeekMap with ID {dto.WeekMapID} does not exist.", null);

            if (weekMap.UserID != userId)
                return (false, "Unauthorized access.", null);

            if (weekMap.DayStartTime > dto.StartTime || weekMap.DayEndTime < dto.EndTime)
                return (false, "Activity start or end time is outside the PlannedWeekMap time range.", null);

            var entity = _mapper.Map<Models.WeekMapActivity>(dto);

            entity.ActivityTemplateID = dto.ActivityTemplateID;
            entity.WeekMapID = dto.WeekMapID;

            _context.WeekMapActivities.Add(entity);
            await _context.SaveChangesAsync();

            return (true, null, entity.WeekMapActivityID);
        }

        public async Task<(bool ok, string? errorMessage)> UpdateAsync(long userId, long id, WeekMapActivityDTO dto)
        {
            var activity = await _context.WeekMapActivities
                .Include(a => a.WeekMap)
                .FirstOrDefaultAsync(a => a.WeekMapActivityID == id);

            if (activity == null)
                return (false, "Activity not found.");

            if (activity.WeekMap == null || activity.WeekMap.UserID != userId)
                return (false, "Unauthorized access.");

            var targetWeekMapId = dto.WeekMapID != 0 ? dto.WeekMapID : activity.WeekMapID;

            var weekMap = await _context.WeekMaps
                .FirstOrDefaultAsync(wm => wm.WeekMapID == targetWeekMapId);

            if (weekMap == null)
                return (false, $"PlannedWeekMap with ID {targetWeekMapId} does not exist.");

            if (weekMap.UserID != userId)
                return (false, "Unauthorized access.");

            if (dto.ActivityTemplateID != 0)
            {
                var templateExists = await _context.ActivityTemplates
                    .AnyAsync(a => a.ActivityTemplateID == dto.ActivityTemplateID);

                if (!templateExists)
                    return (false, $"ActivityTemplate with ID {dto.ActivityTemplateID} does not exist.");
            }

            if (weekMap.DayStartTime > dto.StartTime || weekMap.DayEndTime < dto.EndTime)
                return (false, "Activity start or end time is outside the PlannedWeekMap time range.");

            _mapper.Map(dto, activity);

            activity.WeekMapID = targetWeekMapId;
            activity.ActivityTemplateID = dto.ActivityTemplateID;

            await _context.SaveChangesAsync();
            return (true, null);
        }

        public async Task<(bool ok, string? errorMessage)> DeleteAsync(long userId, long id)
        {
            var activity = await _context.WeekMapActivities
                .Include(a => a.WeekMap)
                .FirstOrDefaultAsync(a => a.WeekMapActivityID == id);

            if (activity == null)
                return (false, "Activity not found.");

            if (activity.WeekMap == null || activity.WeekMap.UserID != userId)
                return (false, "Unauthorized access.");

            _context.WeekMapActivities.Remove(activity);
            await _context.SaveChangesAsync();

            return (true, null);
        }
    }
}
