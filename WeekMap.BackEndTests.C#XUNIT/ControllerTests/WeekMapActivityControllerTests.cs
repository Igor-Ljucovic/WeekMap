using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;
using FluentAssertions;
using WeekMap.DTOs;
using WeekMap;
using XUnitTests.TestData;
using XUnitTests.Utils;
using WeekMap.Models;
using System.Collections.Generic;

namespace XUnitTests.Controllers
{
    public class WeekMapActivityControllerTests
    {
        private readonly WeekMapActivityTestData _plannedWeekMapActivityTestData = new WeekMapActivityTestData();

        [Fact]
        public async Task GetPlannedWeekMapActivity()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            var response = await client.GetAsync($"api/WeekMapActivity/1");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            var plannedWeekMapActivityDTO = _plannedWeekMapActivityTestData.Valid.ElementAt(0);
            plannedWeekMapActivityDTO.WeekMapID = 1;
            plannedWeekMapActivityDTO.ActivityTemplateID = 1;
            response = await client.PostAsJsonAsync("api/WeekMapActivity", plannedWeekMapActivityDTO);
            // Not found because there are no ActivityTemplates with id 1 and no PlannedWeekMaps with id 1
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            // creating the necessary rows for the aggregate table to be added
            var (plannedWeekMapID, activityID) = await TestDataSeeder.SeedPlannedWeekMapActivityAsync(client);

            plannedWeekMapActivityDTO.WeekMapID = plannedWeekMapID;
            plannedWeekMapActivityDTO.ActivityTemplateID = activityID;

            response = await client.PostAsJsonAsync("api/WeekMapActivity", plannedWeekMapActivityDTO);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            // OK because now we have an ActivityTemplate and a PlannedWeekMap with the necessary id,
            // since this is an aggregate table, we need objects with both of the passed ids to exist
            var lista = await client.GetFromJsonAsync<WeekMapActivity>($"api/WeekMapActivity/1");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task PostPlannedWeekMapActivity()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            HttpResponseMessage? response;
            int increment = 1;

            // creating the necessary rows for the aggregate table to be added
            var (plannedWeekMapID, activityID) = await TestDataSeeder.SeedPlannedWeekMapActivityAsync(client);

            foreach (var plannedWeekMapActivity in _plannedWeekMapActivityTestData.Valid)
            {
                plannedWeekMapActivity.WeekMapID = plannedWeekMapID;
                plannedWeekMapActivity.ActivityTemplateID = activityID;

                response = await client.PostAsJsonAsync($"api/WeekMapActivity", plannedWeekMapActivity);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                response = await client.DeleteAsync($"api/WeekMapActivity/{increment++}");
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            }

            foreach (var plannedWeekMapActivity in _plannedWeekMapActivityTestData.Invalid)
            {
                plannedWeekMapActivity.WeekMapID = plannedWeekMapID;
                plannedWeekMapActivity.ActivityTemplateID = activityID;

                response = await client.PostAsJsonAsync($"api/WeekMapActivity", plannedWeekMapActivity);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task PutPlannedWeekMapActivity()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            HttpResponseMessage? response;
            WeekMapActivityDTO updatedPlannedWeekMapActivity;

            // creating the necessary rows for the aggregate table to be added
            var (plannedWeekMapID, activityID) = await TestDataSeeder.SeedPlannedWeekMapActivityAsync(client);

            var plannedWeekMapActivityDTO = _plannedWeekMapActivityTestData.Valid.ElementAt(0);
            plannedWeekMapActivityDTO.WeekMapID = plannedWeekMapID;
            plannedWeekMapActivityDTO.ActivityTemplateID = activityID;
            response = await client.PostAsJsonAsync("api/WeekMapActivity", plannedWeekMapActivityDTO);

            foreach (var plannedWeekMapActivity in _plannedWeekMapActivityTestData.Valid)
            {
                updatedPlannedWeekMapActivity = plannedWeekMapActivity;
                response = await client.PutAsJsonAsync($"api/WeekMapActivity/1", plannedWeekMapActivity);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                WeekMapActivityDTO afterEditPlannedWeekMapActivity = await client.GetFromJsonAsync<WeekMapActivityDTO>($"api/WeekMapActivity/1");
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
                plannedWeekMapActivity.WeekMapID = plannedWeekMapID;
                plannedWeekMapActivity.ActivityTemplateID = activityID;

                response = await client.PutAsJsonAsync($"api/WeekMapActivity/1", plannedWeekMapActivity);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

                response = await client.GetAsync($"api/WeekMapActivity/1");
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
            plannedWeekMapActivityDTO.WeekMapID = plannedWeekMapID;
            plannedWeekMapActivityDTO.ActivityTemplateID = activityID;

            response = await client.PostAsJsonAsync("api/WeekMapActivity", plannedWeekMapActivityDTO);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            response = await client.GetAsync($"api/WeekMapActivity/1");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            response = await client.DeleteAsync($"api/WeekMapActivity/1");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
