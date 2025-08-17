using System.Net.Http.Json;
using WeekMap.Models;
using WeekMap.DTOs;

namespace XUnitTests.Utils
{
    public static class TestDataSeeder
    {
        private static readonly PlannedWeekMapTestData _plannedWeekMapTestData = new PlannedWeekMapTestData();
        private static readonly ActivityTestData _activityTestData = new ActivityTestData();

        public static async Task<(long plannedWeekMapId, long activityId)> SeedPlannedWeekMapActivityAsync(HttpClient client)
        {
            var plannedWeekMapDTO = _plannedWeekMapTestData.Valid.ElementAt(0);
            plannedWeekMapDTO.DayStartTime = new TimeSpan(0, 0, 0);
            plannedWeekMapDTO.DayEndTime = new TimeSpan(23, 59, 0);

            var response = await client.PostAsJsonAsync("api/PlannedWeekMap", plannedWeekMapDTO);
            response.EnsureSuccessStatusCode();

            var plannedWeekMapList = await client.GetFromJsonAsync<List<PlannedWeekMap>>("api/PlannedWeekMap");
            var plannedWeekMapID = plannedWeekMapList!.First().PlannedWeekMapID;

            var activityDTO = _activityTestData.Valid.ElementAt(0);

            response = await client.PostAsJsonAsync("api/Activity", activityDTO);
            response.EnsureSuccessStatusCode();

            var activityList = await client.GetFromJsonAsync<List<Activity>>("api/Activity");
            var activityID = activityList!.First().ActivityID;

            return (plannedWeekMapID, activityID);
        }
    }
}