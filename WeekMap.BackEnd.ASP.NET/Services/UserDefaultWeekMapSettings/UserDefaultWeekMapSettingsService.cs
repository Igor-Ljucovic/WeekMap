using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeekMap.Data;
using WeekMap.DTOs;
using Models = WeekMap.Models;

namespace WeekMap.Services.UserDefaultWeekMapSettings
{
    public class UserDefaultWeekMapSettingsService : IUserDefaultWeekMapSettingsService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserDefaultWeekMapSettingsService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDefaultWeekMapSettingsDTO?> GetByUserIdAsync(long userId)
        {
            var settings = await _context.UserDefaultWeekMapSettings
                .FirstOrDefaultAsync(wms => wms.UserID == userId);

            if (settings == null) return null;

            return _mapper.Map<UserDefaultWeekMapSettingsDTO>(settings);
        }

        public async Task<bool> UpdateAsync(long userId, UserDefaultWeekMapSettingsDTO dto)
        {
            var settings = await _context.UserDefaultWeekMapSettings
                .FirstOrDefaultAsync(wms => wms.UserID == userId);

            if (settings == null) return false;

            _mapper.Map(dto, settings);
            settings.UserID = userId;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
