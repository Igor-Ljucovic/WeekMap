using System.Net.Http.Json;
using WeekMap.Models;
using WeekMap.DTOs;
using Models = WeekMap.Models;

namespace XUnitTests.Utils
{
    public static class TestDataSeeder
    {
        private static readonly WeekMapTestData _plannedWeekMapTestData = new WeekMapTestData();
        private static readonly ActivityTemplateTestData _activityTestData = new ActivityTemplateTestData();

        public static async Task<(long plannedWeekMapId, long activityId)> SeedPlannedWeekMapActivityAsync(HttpClient client)
        {
            var plannedWeekMapDTO = _plannedWeekMapTestData.Valid.ElementAt(0);
            plannedWeekMapDTO.DayStartTime = new TimeSpan(0, 0, 0);
            plannedWeekMapDTO.DayEndTime = new TimeSpan(23, 59, 0);

            var response = await client.PostAsJsonAsync("api/WeekMap", plannedWeekMapDTO);
            response.EnsureSuccessStatusCode();

            var plannedWeekMapList = await client.GetFromJsonAsync<List<Models.WeekMap>>("api/WeekMap");
            var plannedWeekMapID = plannedWeekMapList!.First().WeekMapID;

            var activityDTO = _activityTestData.Valid.ElementAt(0);

            response = await client.PostAsJsonAsync("api/ActivityTemplate", activityDTO);
            response.EnsureSuccessStatusCode();

            var activityList = await client.GetFromJsonAsync<List<ActivityTemplate>>("api/ActivityTemplate");
            var activityID = activityList!.First().ActivityTemplateID;

            return (plannedWeekMapID, activityID);
        }
    }
}