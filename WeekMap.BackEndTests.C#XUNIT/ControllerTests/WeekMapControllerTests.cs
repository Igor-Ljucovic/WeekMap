using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;
using FluentAssertions;
using WeekMap.DTOs;
using WeekMap;
using XUnitTests.TestData;
using WeekMap.Models;

namespace XUnitTests.Controllers
{
    public class WeekMapControllerTests
    {
        private readonly WeekMapTestData _plannedWeekMapTestData = new WeekMapTestData();

        [Fact]
        public async Task GetPlannedWeekMap()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            var response = await client.GetAsync("api/WeekMap");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var plannedWeekMap = _plannedWeekMapTestData.Valid.ElementAt(0);
            await client.PostAsJsonAsync("api/WeekMap", plannedWeekMap);

            var plannedWeekMaps = await client.GetFromJsonAsync<List<WeekMapDTO>>("api/WeekMap");
            plannedWeekMaps.Should().ContainSingle(m =>
                m.Name == plannedWeekMap.Name &&
                m.DayStartTime == plannedWeekMap.DayStartTime &&
                m.DayEndTime == plannedWeekMap.DayEndTime &&
                m.ShowPlaceInPreview == plannedWeekMap.ShowPlaceInPreview &&
                m.ShowDescriptionInPreview == plannedWeekMap.ShowDescriptionInPreview &&
                m.UserID == plannedWeekMap.UserID
            );
        }

        [Fact]
        public async Task PostPlannedWeekMap()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            HttpResponseMessage? response;
            List<WeekMapDTO>? plannedWeekMaps;

            foreach (var plannedWeekMap in _plannedWeekMapTestData.Valid)
            {
                response = await client.PostAsJsonAsync("api/WeekMap", plannedWeekMap);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                response = await client.GetAsync("/api/WeekMap");
                plannedWeekMaps = await response.Content.ReadFromJsonAsync<List<WeekMapDTO>>();

                plannedWeekMaps.Should().ContainSingle(m =>
                    m.Name == plannedWeekMap.Name &&
                    m.DayStartTime == plannedWeekMap.DayStartTime &&
                    m.DayEndTime == plannedWeekMap.DayEndTime &&
                    m.ShowPlaceInPreview == plannedWeekMap.ShowPlaceInPreview &&
                    m.ShowDescriptionInPreview == plannedWeekMap.ShowDescriptionInPreview &&
                    m.UserID == plannedWeekMap.UserID
                );

                long? id = plannedWeekMaps?.First().WeekMapID;
                response = await client.DeleteAsync($"api/WeekMap/{id}");
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            }

            foreach (var plannedWeekMap in _plannedWeekMapTestData.Invalid)
            {
                response = await client.PostAsJsonAsync("api/WeekMap", plannedWeekMap);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

                response = await client.GetAsync("api/WeekMap");
                plannedWeekMaps = await response.Content.ReadFromJsonAsync<List<WeekMapDTO>>();
                plannedWeekMaps.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task PutPlannedWeekMap()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            List<WeekMapDTO>? plannedWeekMaps;

            WeekMapDTO plannedWeekMapDTO = new WeekMapDTO
            {
                Name = "name1",
                DayStartTime = new TimeSpan(8, 0, 0),
                DayEndTime = new TimeSpan(16, 0, 0),
                ShowPlaceInPreview = true,
                ShowDescriptionInPreview = true,
                UserID = 1
            };

            HttpResponseMessage? response = await client.PostAsJsonAsync("/api/WeekMap", plannedWeekMapDTO);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var existingMaps = await client.GetFromJsonAsync<List<WeekMapDTO>>("/api/WeekMap");
            var id = existingMaps.First().WeekMapID;

            foreach (var plannedWeekMap in _plannedWeekMapTestData.Valid)
            {
                var updatedPlannedWeekMap = plannedWeekMap;
                response = await client.PutAsJsonAsync($"/api/WeekMap/{id}", updatedPlannedWeekMap);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                var afterEditPlannedWeekMap = await client.GetFromJsonAsync<List<WeekMapDTO>>("/api/WeekMap");
                var editedMap = afterEditPlannedWeekMap?.FirstOrDefault(m => m.WeekMapID == id);

                editedMap?.Name.Should().Be(updatedPlannedWeekMap.Name);
                editedMap?.DayStartTime.Should().Be(updatedPlannedWeekMap.DayStartTime);
                editedMap?.DayEndTime.Should().Be(updatedPlannedWeekMap.DayEndTime);
                editedMap?.ShowPlaceInPreview.Should().Be(updatedPlannedWeekMap.ShowPlaceInPreview);
                editedMap?.ShowDescriptionInPreview.Should().Be(updatedPlannedWeekMap.ShowDescriptionInPreview);
                editedMap?.UserID.Should().Be(updatedPlannedWeekMap.UserID);
            }

            await client.PutAsJsonAsync($"api/WeekMap/{id}", plannedWeekMapDTO);

            foreach (var plannedWeekMap in _plannedWeekMapTestData.Invalid)
            {
                response = await client.PutAsJsonAsync($"api/WeekMap/{id}", plannedWeekMap);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

                response = await client.GetAsync("/api/WeekMap");
                plannedWeekMaps = await response.Content.ReadFromJsonAsync<List<WeekMapDTO>>();

                plannedWeekMaps.Should().ContainSingle(m =>
                    m.WeekMapID == id &&
                    m.Name == plannedWeekMapDTO.Name &&
                    m.DayStartTime == plannedWeekMapDTO.DayStartTime &&
                    m.DayEndTime == plannedWeekMapDTO.DayEndTime &&
                    m.ShowPlaceInPreview == plannedWeekMapDTO.ShowPlaceInPreview &&
                    m.ShowDescriptionInPreview == plannedWeekMapDTO.ShowDescriptionInPreview &&
                    m.UserID == plannedWeekMapDTO.UserID
                );
            }
        }

        [Fact]
        public async Task DeletePlannedWeekMap()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            var plannedWeekMap = _plannedWeekMapTestData.Valid.ElementAt(0);
            await client.PostAsJsonAsync("api/WeekMap", plannedWeekMap);
            var list = await client.GetFromJsonAsync<List<WeekMapDTO>>("api/WeekMap");
            var id = list?.First().WeekMapID;

            var response = await client.DeleteAsync($"api/WeekMap/{id}");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
