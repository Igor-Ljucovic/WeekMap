using WeekMap.DTOs;
using XUnitTests.TestData;

public class RealisedWeekMapTestData : ITestData<RealisedWeekMapDTO>
{
    public IEnumerable<RealisedWeekMapDTO> Valid =>
    new List<RealisedWeekMapDTO>
    {
        new RealisedWeekMapDTO { DateCreated = DateTime.UtcNow, PlannedWeekMapID = 1 },
    };
    

    public IEnumerable<RealisedWeekMapDTO> Invalid =>
    new List<RealisedWeekMapDTO>()
    {
        // no elements because these attributes can't have invalid values
    };
    
}