using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;
using FluentAssertions;
using WeekMap.DTOs;
using XUnitTests.TestData;
using WeekMap;

namespace XUnitTests.Controllers
{
    public class ActivityTemplateControllerTests
    {
        private readonly ActivityTemplateTestData _activityTestData = new ActivityTemplateTestData();

        [Fact]
        public async Task GetActivity()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            var response = await client.GetAsync("api/ActivityTemplate");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var activity = _activityTestData.Valid.ElementAt(0);
            await client.PostAsJsonAsync("api/ActivityTemplate", activity);

            var activities = await client.GetFromJsonAsync<List<ActivityTemplateDTO>>("api/ActivityTemplate");
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
            List<ActivityTemplateDTO>? activities;

            foreach (var activity in _activityTestData.Valid)
            {
                response = await client.PostAsJsonAsync("api/ActivityTemplate", activity);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                response = await client.GetAsync("/api/ActivityTemplate");
                activities = await response.Content.ReadFromJsonAsync<List<ActivityTemplateDTO>>();

                activities.Should().ContainSingle(a => a.Name == activity.Name);
                activities.Should().ContainSingle(a => a.Description == activity.Description);
                activities.Should().ContainSingle(a => a.UserID == activity.UserID);
                activities.Should().ContainSingle(a => a.ActivityCategoryID == activity.ActivityCategoryID);

                long? id = activities?.First().ActivityTemplateID;
                response = await client.DeleteAsync($"api/ActivityTemplate/{id}");
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            }

            foreach (var activity in _activityTestData.Invalid)
            {
                response = await client.PostAsJsonAsync("api/ActivityTemplate", activity);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

                response = await client.GetAsync("api/ActivityTemplate");
                activities = await response.Content.ReadFromJsonAsync<List<ActivityTemplateDTO>>();
                activities.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task PutActivity()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            List<ActivityTemplateDTO>? activities;

            ActivityTemplateDTO activityDTO = new ActivityTemplateDTO { Name = "beforeEditName", Description = "beforeEditDescription", UserID = 1, ActivityCategoryID = 1 };
            HttpResponseMessage? response = await client.PostAsJsonAsync("/api/ActivityTemplate", activityDTO);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var categories = await client.GetFromJsonAsync<List<ActivityTemplateDTO>>("/api/ActivityTemplate");
            var id = categories?.First().ActivityTemplateID;

            foreach (var activity in _activityTestData.Valid)
            {
                var updatedActivity = activity;
                response = await client.PutAsJsonAsync($"/api/ActivityTemplate/{id}", updatedActivity);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                var afterEditCategories = await client.GetFromJsonAsync<List<ActivityTemplateDTO>>("/api/ActivityTemplate");
                var editedActivity = afterEditCategories?.FirstOrDefault(c => c.ActivityTemplateID == id);
                editedActivity?.Name.Should().Be(updatedActivity.Name);
                editedActivity?.Description.Should().Be(updatedActivity.Description);
            }

            await client.PutAsJsonAsync($"api/ActivityTemplate/{id}", activityDTO);

            foreach (var activity in _activityTestData.Invalid)
            {
                response = await client.PutAsJsonAsync($"api/ActivityTemplate/{id}", activity);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

                response = await client.GetAsync("/api/ActivityTemplate");
                activities = await response.Content.ReadFromJsonAsync<List<ActivityTemplateDTO>>();
                activities.Should().ContainSingle(a => a.Name == "beforeEditName" && a.Description == "beforeEditDescription");
            }
        }

        [Fact]
        public async Task DeleteActivity()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            var activity = _activityTestData.Valid.ElementAt(0);
            await client.PostAsJsonAsync("api/ActivityTemplate", activity);
            var list = await client.GetFromJsonAsync<List<ActivityTemplateDTO>>("api/ActivityTemplate");
            var id = list?.First().ActivityTemplateID;

            var response = await client.DeleteAsync($"api/ActivityTemplate/{id}");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
