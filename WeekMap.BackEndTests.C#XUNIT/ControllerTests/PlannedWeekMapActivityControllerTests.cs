using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;
using FluentAssertions;
using WeekMap.DTOs;
using WeekMap;
using XUnitTests.TestData;
using XUnitTests.Utils;

namespace XUnitTests.Controllers
{
    public class PlannedWeekMapActivityControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly PlannedWeekMapActivityTestData _plannedWeekMapActivityTestData = new PlannedWeekMapActivityTestData();

        public PlannedWeekMapActivityControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetPlannedWeekMapActivity()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            var response = await client.GetAsync($"api/PlannedWeekMapActivity/1/1");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            var plannedWeekMapActivityDTO = _plannedWeekMapActivityTestData.Valid.ElementAt(0);
            plannedWeekMapActivityDTO.PlannedWeekMapID = 1;
            plannedWeekMapActivityDTO.ActivityID = 1;
            response = await client.PostAsJsonAsync("api/PlannedWeekMapActivity", plannedWeekMapActivityDTO);
            // Not found because there are no Activities with id 1 and no PlannedWeekMaps with id 1
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            // creating the necessary rows for the aggregate table to be added
            var (plannedWeekMapID, activityID) = await TestDataSeeder.SeedPlannedWeekMapActivityAsync(client);

            plannedWeekMapActivityDTO.PlannedWeekMapID = plannedWeekMapID;
            plannedWeekMapActivityDTO.ActivityID = activityID;

            response = await client.PostAsJsonAsync("api/PlannedWeekMapActivity", plannedWeekMapActivityDTO);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            // OK because now we have an Activity and a PlannedWeekMap with the necessary id,
            // since this is an aggregate table, we need objects with both of the passed ids to exist
            response = await client.GetAsync($"api/PlannedWeekMapActivity/{plannedWeekMapActivityDTO.PlannedWeekMapID}/{plannedWeekMapActivityDTO.ActivityID}");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task PostPlannedWeekMapActivity()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            HttpResponseMessage? response;

            // creating the necessary rows for the aggregate table to be added
            var (plannedWeekMapID, activityID) = await TestDataSeeder.SeedPlannedWeekMapActivityAsync(client);

            foreach (var plannedWeekMapActivity in _plannedWeekMapActivityTestData.Valid)
            {
                plannedWeekMapActivity.PlannedWeekMapID = plannedWeekMapID;
                plannedWeekMapActivity.ActivityID = activityID;

                response = await client.PostAsJsonAsync($"api/PlannedWeekMapActivity", plannedWeekMapActivity);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                response = await client.DeleteAsync($"api/PlannedWeekMapActivity/{plannedWeekMapActivity.PlannedWeekMapID}/{plannedWeekMapActivity.ActivityID}");
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            }

            foreach (var plannedWeekMapActivity in _plannedWeekMapActivityTestData.Invalid)
            {
                plannedWeekMapActivity.PlannedWeekMapID = plannedWeekMapID;
                plannedWeekMapActivity.ActivityID = activityID;

                response = await client.PostAsJsonAsync($"api/PlannedWeekMapActivity", plannedWeekMapActivity);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task PutPlannedWeekMapActivity()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            HttpResponseMessage? response;
            PlannedWeekMapActivityDTO updatedPlannedWeekMapActivity;

            // creating the necessary rows for the aggregate table to be added
            var (plannedWeekMapID, activityID) = await TestDataSeeder.SeedPlannedWeekMapActivityAsync(client);

            var plannedWeekMapActivityDTO = _plannedWeekMapActivityTestData.Valid.ElementAt(0);
            plannedWeekMapActivityDTO.PlannedWeekMapID = plannedWeekMapID;
            plannedWeekMapActivityDTO.ActivityID = activityID;
            response = await client.PostAsJsonAsync("api/PlannedWeekMapActivity", plannedWeekMapActivityDTO);

            foreach (var plannedWeekMapActivity in _plannedWeekMapActivityTestData.Valid)
            {
                updatedPlannedWeekMapActivity = plannedWeekMapActivity;
                response = await client.PutAsJsonAsync($"api/PlannedWeekMapActivity/{plannedWeekMapActivityDTO.PlannedWeekMapID}/{plannedWeekMapActivityDTO.ActivityID}", plannedWeekMapActivity);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                PlannedWeekMapActivityDTO afterEditPlannedWeekMapActivity = await client.GetFromJsonAsync<PlannedWeekMapActivityDTO>($"api/PlannedWeekMapActivity/{plannedWeekMapActivity.PlannedWeekMapID}/{plannedWeekMapActivity.ActivityID}");
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                afterEditPlannedWeekMapActivity!.StartTime.Should().Be(updatedPlannedWeekMapActivity.StartTime);
                afterEditPlannedWeekMapActivity.EndTime.Should().Be(updatedPlannedWeekMapActivity.EndTime);
                afterEditPlannedWeekMapActivity.OnMonday.Should().Be(updatedPlannedWeekMapActivity.OnMonday);
                afterEditPlannedWeekMapActivity.OnTuesday.Should().Be(updatedPlannedWeekMapActivity.OnTuesday);
                afterEditPlannedWeekMapActivity.OnWednesday.Should().Be(updatedPlannedWeekMapActivity.OnWednesday);
                afterEditPlannedWeekMapActivity.OnThursday.Should().Be(updatedPlannedWeekMapActivity.OnThursday);
                afterEditPlannedWeekMapActivity.OnFriday.Should().Be(updatedPlannedWeekMapActivity.OnFriday);
                afterEditPlannedWeekMapActivity.OnSaturday.Should().Be(updatedPlannedWeekMapActivity.OnSaturday);
                afterEditPlannedWeekMapActivity.OnSunday.Should().Be(updatedPlannedWeekMapActivity.OnSunday);
            }

            foreach (var plannedWeekMapActivity in _plannedWeekMapActivityTestData.Invalid)
            {
                updatedPlannedWeekMapActivity = plannedWeekMapActivity;
                plannedWeekMapActivity.PlannedWeekMapID = plannedWeekMapID;
                plannedWeekMapActivity.ActivityID = activityID;

                response = await client.PutAsJsonAsync($"api/PlannedWeekMapActivity/{plannedWeekMapActivity.PlannedWeekMapID}/{plannedWeekMapActivity.ActivityID}", plannedWeekMapActivity);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

                response = await client.GetAsync($"api/PlannedWeekMapActivity/{plannedWeekMapActivity.PlannedWeekMapID}/{plannedWeekMapActivity.ActivityID}");
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task DeletePlannedWeekMapActivity()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            HttpResponseMessage? response;

            // creating the necessary rows for the aggregate table to be added
            var (plannedWeekMapID, activityID) = await TestDataSeeder.SeedPlannedWeekMapActivityAsync(client);

            var plannedWeekMapActivityDTO = _plannedWeekMapActivityTestData.Valid.ElementAt(0);
            plannedWeekMapActivityDTO.PlannedWeekMapID = plannedWeekMapID;
            plannedWeekMapActivityDTO.ActivityID = activityID;

            response = await client.PostAsJsonAsync("api/PlannedWeekMapActivity", plannedWeekMapActivityDTO);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            response = await client.DeleteAsync($"api/PlannedWeekMapActivity/{plannedWeekMapActivityDTO.PlannedWeekMapID}/{plannedWeekMapActivityDTO.ActivityID}");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
