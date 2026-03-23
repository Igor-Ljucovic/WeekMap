using AutoMapper;
using WeekMap.DTOs;
using WeekMap.Repositories.UserSettings;

namespace WeekMap.Services.UserSettings
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly IUserSettingsRepository _repo;
        private readonly IMapper _mapper;

        public UserSettingsService(IUserSettingsRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UserSettingsDTO?> GetByUserIdAsync(long userId)
        {
            var settings = await _repo.GetByUserIdAsync(userId);
            if (settings == null) return null;

            return _mapper.Map<UserSettingsDTO>(settings);
        }

        public async Task<bool> UpdateAsync(long userId, UserSettingsDTO dto)
        {
            var settings = await _repo.GetByUserIdAsync(userId);
            if (settings == null) return false;

            _mapper.Map(dto, settings);
            settings.UserID = userId;

            await _repo.SaveChangesAsync();
            return true;
        }
    }
}