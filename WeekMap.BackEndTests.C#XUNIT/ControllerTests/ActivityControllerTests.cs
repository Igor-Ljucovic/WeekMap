using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;
using FluentAssertions;
using WeekMap.DTOs;
using XUnitTests.TestData;
using WeekMap;

namespace XUnitTests.Controllers
{
    public class ActivityControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly ActivityTestData _activityTestData;

        public ActivityControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _activityTestData = new ActivityTestData();
        }

        [Fact]
        public async Task GetActivity()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            var response = await client.GetAsync("api/Activity");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var activity = _activityTestData.Valid.ElementAt(0);
            await client.PostAsJsonAsync("api/Activity", activity);

            var activities = await client.GetFromJsonAsync<List<ActivityDTO>>("api/Activity");
            activities.Should().ContainSingle(a => a.Name == activity.Name);
            activities.Should().ContainSingle(a => a.Description == activity.Description);
            activities.Should().ContainSingle(a => a.UserID == activity.UserID);
            activities.Should().ContainSingle(a => a.ActivityCategoryID == activity.ActivityCategoryID);
        }

        [Fact]
        public async Task PostActivity()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            HttpResponseMessage? response;
            List<ActivityDTO>? activities;

            foreach (var activity in _activityTestData.Valid)
            {
                response = await client.PostAsJsonAsync("api/Activity", activity);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                response = await client.GetAsync("/api/Activity");
                activities = await response.Content.ReadFromJsonAsync<List<ActivityDTO>>();

                activities.Should().ContainSingle(a => a.Name == activity.Name);
                activities.Should().ContainSingle(a => a.Description == activity.Description);
                activities.Should().ContainSingle(a => a.UserID == activity.UserID);
                activities.Should().ContainSingle(a => a.ActivityCategoryID == activity.ActivityCategoryID);

                long? id = activities?.First().ActivityID;
                response = await client.DeleteAsync($"api/Activity/{id}");
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            }

            foreach (var activity in _activityTestData.Invalid)
            {
                response = await client.PostAsJsonAsync("api/Activity", activity);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

                response = await client.GetAsync("api/Activity");
                activities = await response.Content.ReadFromJsonAsync<List<ActivityDTO>>();
                activities.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task PutActivity()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            List<ActivityDTO>? activities;

            ActivityDTO activityDTO = new ActivityDTO { Name = "beforeEditName", Description = "beforeEditDescription", UserID = 1, ActivityCategoryID = 1 };
            HttpResponseMessage? response = await client.PostAsJsonAsync("/api/Activity", activityDTO);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var categories = await client.GetFromJsonAsync<List<ActivityDTO>>("/api/Activity");
            var id = categories?.First().ActivityID;

            foreach (var activity in _activityTestData.Valid)
            {
                var updatedActivity = activity;
                response = await client.PutAsJsonAsync($"/api/Activity/{id}", updatedActivity);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                var afterEditCategories = await client.GetFromJsonAsync<List<ActivityDTO>>("/api/Activity");
                var editedActivity = afterEditCategories?.FirstOrDefault(c => c.ActivityID == id);
                editedActivity?.Name.Should().Be(updatedActivity.Name);
                editedActivity?.Description.Should().Be(updatedActivity.Description);
            }

            await client.PutAsJsonAsync($"api/Activity/{id}", activityDTO);

            foreach (var activity in _activityTestData.Invalid)
            {
                response = await client.PutAsJsonAsync($"api/Activity/{id}", activity);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

                response = await client.GetAsync("/api/Activity");
                activities = await response.Content.ReadFromJsonAsync<List<ActivityDTO>>();
                activities.Should().ContainSingle(a => a.Name == "beforeEditName" && a.Description == "beforeEditDescription");
            }
        }

        [Fact]
        public async Task DeleteActivity()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            var activity = _activityTestData.Valid.ElementAt(0);
            await client.PostAsJsonAsync("api/Activity", activity);
            var list = await client.GetFromJsonAsync<List<ActivityDTO>>("api/Activity");
            var id = list?.First().ActivityID;

            var response = await client.DeleteAsync($"api/Activity/{id}");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
