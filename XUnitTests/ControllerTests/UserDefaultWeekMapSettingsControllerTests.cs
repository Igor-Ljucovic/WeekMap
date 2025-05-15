using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;
using WeekMap;
using FluentAssertions;
using WeekMap.DTOs;
using XUnitTests.TestData;
using WeekMap.Models;

namespace XUnitTests.Controllers
{

    public class UserDefaultWeekMapSettingsControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly UserDefaultWeekMapSettingsTestData _userDefaultWeekMapSettingsTestData;

        public UserDefaultWeekMapSettingsControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _userDefaultWeekMapSettingsTestData = new UserDefaultWeekMapSettingsTestData();
        }

        [Fact]
        public async Task GetUserDefaultWeekMapSettings()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            var response = await client.GetAsync("api/users");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();
            users.Should().NotBeNullOrEmpty();
            var id = users?.First().UserID;

            response = await client.GetAsync($"api/UserDefaultWeekMapSettings/{id}");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var userSettings = await response.Content.ReadFromJsonAsync<UserDefaultWeekMapSettingsDTO>();
            userSettings.Should().NotBeNull();

            // because we want to get the default settings
            UserDefaultWeekMapSettings userSettingsSample = new UserDefaultWeekMapSettings();

            userSettings!.WeekStartDay.Should().Be(userSettingsSample.WeekStartDay);
            userSettings.SkipSaturday.Should().Be(userSettingsSample.SkipSaturday);
            userSettings.SkipSunday.Should().Be(userSettingsSample.SkipSunday);
            userSettings.DayStartTime.Should().Be(userSettingsSample.DayStartTime);
            userSettings.DayEndTime.Should().Be(userSettingsSample.DayEndTime);
            userSettings.ShowPlaceInPreview.Should().Be(userSettingsSample.ShowPlaceInPreview);
            userSettings.ShowDescriptionInPreview.Should().Be(userSettingsSample.ShowDescriptionInPreview);
        }

        [Fact]
        public async Task PutUserDefaultWeekMapSettings()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            var response = await client.GetAsync("api/users");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();
            users.Should().NotBeNullOrEmpty();
            var id = users?.First().UserID;

            UserDefaultWeekMapSettingsDTO updatedSettings = new UserDefaultWeekMapSettingsDTO
            {
                SkipSaturday = false,
                SkipSunday = true,
                WeekStartDay = "Monday",
                DayStartTime = new TimeSpan(0, 0, 0),
                DayEndTime = new TimeSpan(11, 0, 0),
                ShowPlaceInPreview = false,
                ShowDescriptionInPreview = false
            };

            foreach (var settings in _userDefaultWeekMapSettingsTestData.Valid)
            {
                updatedSettings = settings;
                response = await client.PutAsJsonAsync($"api/UserDefaultWeekMapSettings/{id}", updatedSettings);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                response = await client.GetAsync($"api/UserDefaultWeekMapSettings/{id}");
                var afterEditSettings = await response.Content.ReadFromJsonAsync<UserDefaultWeekMapSettings>();
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                afterEditSettings!.WeekStartDay.Should().Be(updatedSettings.WeekStartDay);
                afterEditSettings.SkipSaturday.Should().Be(updatedSettings.SkipSaturday);
                afterEditSettings.SkipSunday.Should().Be(updatedSettings.SkipSunday);
                afterEditSettings.ShowPlaceInPreview.Should().Be(updatedSettings.ShowPlaceInPreview);
                afterEditSettings.ShowDescriptionInPreview.Should().Be(updatedSettings.ShowDescriptionInPreview);
            }

            foreach (var settings in _userDefaultWeekMapSettingsTestData.Invalid)
            {
                response = await client.PutAsJsonAsync($"api/UserDefaultWeekMapSettings/{id}", settings);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}