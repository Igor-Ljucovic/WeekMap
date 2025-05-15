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

    public class UserSettingsControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly UserSettingsTestData _userSettingsTestData;

        public UserSettingsControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _userSettingsTestData = new UserSettingsTestData();
        }

        [Fact]
        public async Task GetUserSettings()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            var response = await client.GetAsync("api/users");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();
            users.Should().NotBeNullOrEmpty();
            var id = users?.First().UserID;

            response = await client.GetAsync($"api/UserSettings/{id}");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var userSettings = await response.Content.ReadFromJsonAsync<UserSettingsDTO>();
            userSettings.Should().NotBeNull();

            // because we want to get the default settings
            UserSettings userSettingsSample = new UserSettings();

            userSettings!.Theme.Should().Be(userSettingsSample.Theme);
            userSettings.EmailUpdates.Should().Be(userSettingsSample.EmailUpdates);
            userSettings.Notification.Should().Be(userSettingsSample.Notification);
            userSettings.NotificationTime.Should().Be(userSettingsSample.NotificationTime);
        }

        [Fact]
        public async Task PutUserSettings()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            HttpResponseMessage? response;
            UserSettingsDTO? updatedSettings;

            UserSettingsDTO activityDTO = new UserSettingsDTO {
                Theme = "dark",
                Notification = true,
                NotificationTime = new TimeSpan(0, 0, 0),
                EmailUpdates = true
            };

            response = await client.GetAsync("api/users");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();
            users.Should().NotBeNullOrEmpty();
            var id = users?.First().UserID;

            foreach (var settings in _userSettingsTestData.Valid)
            {
                updatedSettings = settings;
                response = await client.PutAsJsonAsync($"api/UserSettings/{id}", updatedSettings);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                response = await client.GetAsync($"api/UserSettings/{id}");
                var afterEditSettings = await response.Content.ReadFromJsonAsync<UserSettingsDTO>();
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                afterEditSettings!.Theme.Should().Be(updatedSettings.Theme);
                afterEditSettings.Notification.Should().Be(updatedSettings.Notification);
                afterEditSettings.NotificationTime.Should().Be(updatedSettings.NotificationTime);
                afterEditSettings.EmailUpdates.Should().Be(updatedSettings.EmailUpdates);
            }

            foreach (var settings in _userSettingsTestData.Invalid)
            {
                response = await client.PutAsJsonAsync($"api/UserSettings/{id}", settings);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}