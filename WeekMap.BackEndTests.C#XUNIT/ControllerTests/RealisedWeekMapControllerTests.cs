using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;
using FluentAssertions;
using WeekMap.DTOs;
using WeekMap;
using XUnitTests.TestData;

namespace XUnitTests.Controllers
{
    public class RealisedWeekMapControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly RealisedWeekMapTestData _realisedWeekMapTestData = new RealisedWeekMapTestData();

        public RealisedWeekMapControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetRealisedWeekMap()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            var response = await client.GetAsync("api/RealisedWeekMap");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var realisedWeekMap = _realisedWeekMapTestData.Valid.ElementAt(0);
            await client.PostAsJsonAsync("api/RealisedWeekMap", realisedWeekMap);

            var realisedWeekMaps = await client.GetFromJsonAsync<List<RealisedWeekMapDTO>>("api/RealisedWeekMap");

            realisedWeekMaps.Should().ContainSingle(r =>
                r.PlannedWeekMapID == realisedWeekMap.PlannedWeekMapID &&
                r.DateCreated.Date == realisedWeekMap.DateCreated.Date
            );
        }

        [Fact]
        public async Task PostRealisedWeekMap()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            HttpResponseMessage? response;
            List<RealisedWeekMapDTO>? realisedWeekMaps;

            foreach (var realisedWeekMap in _realisedWeekMapTestData.Valid)
            {
                response = await client.PostAsJsonAsync("api/RealisedWeekMap", realisedWeekMap);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                response = await client.GetAsync("/api/RealisedWeekMap");
                realisedWeekMaps = await response.Content.ReadFromJsonAsync<List<RealisedWeekMapDTO>>();

                long? id = realisedWeekMaps?.First().PlannedWeekMapID;
                response = await client.DeleteAsync($"api/RealisedWeekMap/{id}");
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            }

            foreach (var realisedWeekMap in _realisedWeekMapTestData.Invalid)
            {
                response = await client.PostAsJsonAsync("api/RealisedWeekMap", realisedWeekMap);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

                response = await client.GetAsync("api/RealisedWeekMap");
                realisedWeekMaps = await response.Content.ReadFromJsonAsync<List<RealisedWeekMapDTO>>();
                realisedWeekMaps.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task DeleteRealisedWeekMap()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            var plannedWeekMap = _realisedWeekMapTestData.Valid.ElementAt(0);
            await client.PostAsJsonAsync("api/RealisedWeekMap", plannedWeekMap);
            var list = await client.GetFromJsonAsync<List<PlannedWeekMapDTO>>("api/RealisedWeekMap");
            var id = list?.First().PlannedWeekMapID;

            var response = await client.DeleteAsync($"api/RealisedWeekMap/{id}");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
