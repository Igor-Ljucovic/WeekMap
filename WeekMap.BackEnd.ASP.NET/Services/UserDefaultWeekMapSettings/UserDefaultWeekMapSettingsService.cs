using AutoMapper;
using WeekMap.DTOs;
using WeekMap.Repositories.UnitOfWork;
using WeekMap.Repositories.UserDefaultWeekMapSettings;

namespace WeekMap.Services.UserDefaultWeekMapSettings
{
    public class UserDefaultWeekMapSettingsService : IUserDefaultWeekMapSettingsService
    {
        private readonly IUserDefaultWeekMapSettingsRepository _repo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserDefaultWeekMapSettingsService(IUserDefaultWeekMapSettingsRepository repo, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDefaultWeekMapSettingsDTO?> GetByUserIdAsync(long userId)
        {
            var settings = await _repo.GetByUserIdAsync(userId);
            if (settings == null) return null;

            return _mapper.Map<UserDefaultWeekMapSettingsDTO>(settings);
        }

        public async Task<bool> UpdateAsync(long userId, UserDefaultWeekMapSettingsDTO dto)
        {
            var settings = await _repo.GetByUserIdAsync(userId);
            if (settings == null) return false;

            _mapper.Map(dto, settings);
            settings.UserID = userId;

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}