using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeekMap.Data;
using WeekMap.DTOs;
using Models = WeekMap.Models;

namespace WeekMap.Services.UserSettings
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserSettingsService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserSettingsDTO?> GetByUserIdAsync(long userId)
        {
            var settings = await _context.UserSettings
                .FirstOrDefaultAsync(us => us.UserID == userId);

            if (settings == null) return null;

            return _mapper.Map<UserSettingsDTO>(settings);
        }

        public async Task<bool> UpdateAsync(long userId, UserSettingsDTO dto)
        {
            var settings = await _context.UserSettings
                .FirstOrDefaultAsync(us => us.UserID == userId);

            if (settings == null) return false;

            _mapper.Map(dto, settings);
            settings.UserID = userId;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
