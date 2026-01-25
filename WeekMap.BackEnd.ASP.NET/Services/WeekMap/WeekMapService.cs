using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeekMap.Data;
using WeekMap.DTOs;
using WeekMap.Models;
using Models = WeekMap.Models;

namespace WeekMap.Services.WeekMap
{
    public class WeekMapService : IWeekMapService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public WeekMapService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Models.WeekMap>> GetAllAsync(long userId)
        {
            return await _context.WeekMaps
                .Where(wm => wm.UserID == userId)
                .ToListAsync();
        }

        public async Task<Models.WeekMap?> GetLatestByUserIdAsync(long sessionUserId, long requestedUserId)
        {
            if (sessionUserId != requestedUserId) return null;

            return await _context.WeekMaps
                .Where(wm => wm.UserID == requestedUserId)
                .OrderByDescending(wm => wm.DateCreated)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Models.WeekMapActivity>> GetWeekMapActivitiesAsync(long sessionUserId, long weekMapId)
        {
            var ownsWeekMap = await _context.WeekMaps
                .AnyAsync(wm => wm.WeekMapID == weekMapId && wm.UserID == sessionUserId);

            if (!ownsWeekMap) return new List<Models.WeekMapActivity>();

            return await _context.WeekMapActivities
                .Include(wma => wma.ActivityTemplate)
                    .ThenInclude(at => at.ActivityCategory)
                .Where(wma => wma.WeekMapID == weekMapId)
                .ToListAsync();
        }

        public async Task<long> CreateAsync(long userId, WeekMapDTO dto)
        {
            dto.UserID = userId;

            var entity = _mapper.Map<Models.WeekMap>(dto);
            entity.UserID = userId;

            _context.WeekMaps.Add(entity);
            await _context.SaveChangesAsync();

            return entity.WeekMapID;
        }

        public async Task<bool> UpdateAsync(long userId, long id, WeekMapDTO dto)
        {
            var map = await _context.WeekMaps
                .FirstOrDefaultAsync(wm => wm.WeekMapID == id && wm.UserID == userId);

            if (map == null) return false;

            _mapper.Map(dto, map);
            map.UserID = userId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long userId, long id)
        {
            var map = await _context.WeekMaps
                .FirstOrDefaultAsync(wm => wm.WeekMapID == id && wm.UserID == userId);

            if (map == null) return false;

            _context.WeekMaps.Remove(map);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
