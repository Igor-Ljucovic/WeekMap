using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeekMap.Data;
using WeekMap.DTOs;

namespace WeekMap.Services.ActivityCategory
{
    public class ActivityCategoryService : IActivityCategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ActivityCategoryService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ActivityCategoryDTO>> GetAllAsync(long userId)
        {
            var categories = await _context.ActivityCategories
                .Where(c => c.UserID == userId)
                .ToListAsync();

            return _mapper.Map<List<ActivityCategoryDTO>>(categories);
        }

        public async Task<long> CreateAsync(long userId, ActivityCategoryDTO dto)
        {
            dto.UserID = userId;

            var entity = _mapper.Map<Models.ActivityCategory>(dto);
            entity.UserID = userId;

            _context.ActivityCategories.Add(entity);
            await _context.SaveChangesAsync();

            return entity.ActivityCategoryID;
        }

        public async Task<bool> UpdateAsync(long userId, long id, ActivityCategoryDTO dto)
        {
            var category = await _context.ActivityCategories
                .FirstOrDefaultAsync(c => c.ActivityCategoryID == id && c.UserID == userId);

            if (category == null) return false;

            _mapper.Map(dto, category);
            category.UserID = userId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long userId, long id)
        {
            var category = await _context.ActivityCategories
                .FirstOrDefaultAsync(c => c.ActivityCategoryID == id && c.UserID == userId);

            if (category == null) return false;

            _context.ActivityCategories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
