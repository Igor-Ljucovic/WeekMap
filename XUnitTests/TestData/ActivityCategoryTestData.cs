using WeekMap.DTOs;
using XUnitTests.TestData;

public class ActivityCategoryTestData : ITestData<ActivityCategoryDTO>
{
    public IEnumerable<ActivityCategoryDTO> Valid =>
        new List<ActivityCategoryDTO>
        {
            new ActivityCategoryDTO { Name = "a", Color = "000000" },
            new ActivityCategoryDTO { Name = "a", Color = "000000" },
            new ActivityCategoryDTO { Name = "1    ", Color = "FFFFFF" },
            new ActivityCategoryDTO { Name = "12345", Color = "123456" },
            new ActivityCategoryDTO { Name = "activity category name 1", Color = "AF3F67" },
            new ActivityCategoryDTO { Name = "! category name 2", Color = "AF3F67" },
            new ActivityCategoryDTO { Name = "č", Color = "AF3F67" },
            new ActivityCategoryDTO { Name = "👍", Color = "AF3F67" },
        };

    public IEnumerable<ActivityCategoryDTO> Invalid =>
        new List<ActivityCategoryDTO>
        {
            new ActivityCategoryDTO { Name = "123456789 123456789 123456789 123456789 123456789 1", Color = "AF3F67" },
            new ActivityCategoryDTO { Name = "", Color = "AF3F67" },
            new ActivityCategoryDTO { Name = "NAME1", Color = "1234567" },
            new ActivityCategoryDTO { Name = "name2", Color = "12345" },
            new ActivityCategoryDTO { Name = "Name3", Color = "12345G" }
        };
}
