using WeekMap.DTOs;
using XUnitTests.TestData;

public class ActivityTemplateTestData : ITestData<ActivityTemplateDTO>
{
    public IEnumerable<ActivityTemplateDTO> Valid =>
    new List<ActivityTemplateDTO>
    {
        new ActivityTemplateDTO
        {
            Name = "activity1",
            Description = "description1",
            ActivityCategoryID = null,
            UserID = 1
        },
        new ActivityTemplateDTO
        {
            Name = "activity2",
            Description = "description1",
            ActivityCategoryID = 1,
            UserID = 1
        },
        new ActivityTemplateDTO
        {
            Name = "empty description",
            Description = "",
            ActivityCategoryID = 1,
            UserID = 1
        },
        new ActivityTemplateDTO
        {
            Name = "null description",
            Description = null,
            ActivityCategoryID = 1,
            UserID = 1
        },
        new ActivityTemplateDTO
        {
            Name = "no description attribute",
            ActivityCategoryID = 1,
            UserID = 1
        },
    };

    public IEnumerable<ActivityTemplateDTO> Invalid =>
    new List<ActivityTemplateDTO>
    {
        new ActivityTemplateDTO
        {
            Name = "",
            Description = "empty name",
            ActivityCategoryID = 1,
            UserID = 1
        },
        new ActivityTemplateDTO
        {
            Name = null,
            Description = "null name",
            ActivityCategoryID = 1,
            UserID = 1
        },

        new ActivityTemplateDTO
        {
            Description = "no name attribute",
            ActivityCategoryID = 1,
            UserID = 1
        },
        new ActivityTemplateDTO
        {

        },
    };
}
