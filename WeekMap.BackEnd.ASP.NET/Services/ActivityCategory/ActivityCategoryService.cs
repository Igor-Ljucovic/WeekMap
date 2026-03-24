using AutoMapper;
using WeekMap.DTOs;
using WeekMap.Repositories.ActivityCategory;
using WeekMap.Repositories.UnitOfWork;

namespace WeekMap.Services.ActivityCategory
{
    public class ActivityCategoryService : IActivityCategoryService
    {
        private readonly IActivityCategoryRepository _repo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ActivityCategoryService(IActivityCategoryRepository repo, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ActivityCategoryDTO>> GetAllAsync(long userId)
        {
            var categories = await _repo.GetAllAsync(userId);
            return _mapper.Map<List<ActivityCategoryDTO>>(categories);
        }

        public async Task<long> CreateAsync(long userId, ActivityCategoryDTO dto)
        {
            dto.UserID = userId;

            var entity = _mapper.Map<Models.ActivityCategory>(dto);
            entity.UserID = userId;

            _repo.Create(entity);

            await _unitOfWork.SaveChangesAsync();

            return entity.ActivityCategoryID;
        }

        public async Task<bool> UpdateAsync(long userId, long id, ActivityCategoryDTO dto)
        {
            var category = await _repo.GetByIdOwnedAsync(userId, id);
            if (category == null) return false;

            _mapper.Map(dto, category);

            category.UserID = userId;

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long userId, long id)
        {
            var category = await _repo.GetByIdOwnedAsync(userId, id);
            if (category == null) return false;

            _repo.Delete(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}