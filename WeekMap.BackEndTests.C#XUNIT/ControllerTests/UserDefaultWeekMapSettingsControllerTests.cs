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

    public class UserDefaultWeekMapSettingsControllerTests
    {
        private readonly UserDefaultWeekMapSettingsTestData _userDefaultWeekMapSettingsTestData = new UserDefaultWeekMapSettingsTestData();

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

            userSettings!.DayStartTime.Should().Be(userSettingsSample.DayStartTime);
            userSettings.DayEndTime.Should().Be(userSettingsSample.DayEndTime);
            userSettings.ShowLocationInPreview.Should().Be(userSettingsSample.ShowLocationInPreview);
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
                DayStartTime = new TimeSpan(0, 0, 0),
                DayEndTime = new TimeSpan(11, 0, 0),
                ShowLocationInPreview = false,
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

                afterEditSettings!.ShowLocationInPreview.Should().Be(updatedSettings.ShowLocationInPreview);
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