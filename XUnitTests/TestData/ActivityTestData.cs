using WeekMap.DTOs;
using XUnitTests.TestData;

public class ActivityTestData : ITestData<ActivityDTO>
{
    public IEnumerable<ActivityDTO> Valid =>
    new List<ActivityDTO>
    {
        new ActivityDTO
        {
            Name = "activity1",
            Description = "description1",
            ActivityCategoryID = null
        },
        new ActivityDTO
        {
            Name = "activity2",
            Description = "description1",
            ActivityCategoryID = 1,
            UserID = 1
        },
        new ActivityDTO
        {
            Name = "empty description",
            Description = "",
            ActivityCategoryID = 1,
            UserID = 1
        },
        new ActivityDTO
        {
            Name = "null description",
            Description = null,
            ActivityCategoryID = 1,
            UserID = 1
        },
        new ActivityDTO
        {
            Name = "no description attribute",
            ActivityCategoryID = 1,
            UserID = 1
        },
    };

    public IEnumerable<ActivityDTO> Invalid =>
    new List<ActivityDTO>
    {
        new ActivityDTO
        {
            Name = "",
            Description = "empty name",
            ActivityCategoryID = 1,
            UserID = 1
        },
        new ActivityDTO
        {
            Name = null,
            Description = "null name",
            ActivityCategoryID = 1,
            UserID = 1
        },

        new ActivityDTO
        {
            Description = "no name attribute",
            ActivityCategoryID = 1,
            UserID = 1
        },
        new ActivityDTO
        {

        },
    };
}
