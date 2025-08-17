using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;
using WeekMap;
using FluentAssertions;
using WeekMap.DTOs;
using XUnitTests.TestData;

namespace XUnitTests.Controllers
{
    // a different in-memory databse is used for every unit test (method),
    // so the tests are independent of each other
    // (the database is created and destroyed for each test)

    public class ActivityCategoryControllerTests
    {
        private readonly ActivityCategoryTestData _activityCategoryTestData = new ActivityCategoryTestData();

        [Fact]
        public async Task GetActivityCategory()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            var response = await client.GetAsync("api/ActivityCategory");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrWhiteSpace();

            ActivityCategoryDTO? category = _activityCategoryTestData.Valid.ElementAt(0);
            response = await client.PostAsJsonAsync("/api/ActivityCategory", category);
            response = await client.GetAsync("api/ActivityCategory");
            List<ActivityCategoryDTO>? categories = await response.Content.ReadFromJsonAsync<List<ActivityCategoryDTO>>();

            categories.Should().ContainSingle(c => c.Name == category.Name);
            categories.Should().ContainSingle(c => c.Color == category.Color);
        }

        [Fact]
        public async Task PostActivityCategory()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            HttpResponseMessage? response;
            List<ActivityCategoryDTO>? categories;

            foreach (var category in _activityCategoryTestData.Valid)
            {
                response = await client.PostAsJsonAsync("/api/ActivityCategory", category);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                response = await client.GetAsync("/api/ActivityCategory");
                categories = await response.Content.ReadFromJsonAsync<List<ActivityCategoryDTO>>();

                categories.Should().ContainSingle(c => c.Name == category.Name);
                categories.Should().ContainSingle(c => c.Color == category.Color);

                var id = categories?.First().ActivityCategoryID;
                response = await client.DeleteAsync($"/api/ActivityCategory/{id}");
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            }

            foreach (var category in _activityCategoryTestData.Invalid)
            {
                response = await client.PostAsJsonAsync("/api/ActivityCategory", category);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

                response = await client.GetAsync("/api/ActivityCategory");
                categories = await response.Content.ReadFromJsonAsync<List<ActivityCategoryDTO>>();
                categories.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task PutActivityCategory()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            ActivityCategoryDTO category = new ActivityCategoryDTO { Name = "beforeEditName", Color = "468ABC" };
            var response = await client.PostAsJsonAsync("/api/ActivityCategory", category);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var categories = await client.GetFromJsonAsync<List<ActivityCategoryDTO>>("/api/ActivityCategory");
            var id = categories?.First().ActivityCategoryID;

            var updatedCategory = new ActivityCategoryDTO { Name = "afterEditName", Color = "FFF369"};
            response = await client.PutAsJsonAsync($"/api/ActivityCategory/{id}", updatedCategory);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var afterEditCategories = await client.GetFromJsonAsync<List<ActivityCategoryDTO>>("/api/ActivityCategory");
            var editedCategory = afterEditCategories?.FirstOrDefault(c => c.ActivityCategoryID == id);
            editedCategory?.Name.Should().Be("afterEditName");
            editedCategory?.Color.Should().Be("FFF369");
        }

        [Fact]
        public async Task DeleteActivityCategory()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            var category = _activityCategoryTestData.Valid.ElementAt(0);
            var response = await client.PostAsJsonAsync("/api/ActivityCategory", category);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var categoriesBefore = await client.GetFromJsonAsync<List<ActivityCategoryDTO>>("/api/ActivityCategory");
            categoriesBefore.Should().ContainSingle(c => c.Name == category.Name);
            categoriesBefore.Should().ContainSingle(c => c.Color == category.Color);
            var categoryId = categoriesBefore?.First().ActivityCategoryID;

            response = await client.DeleteAsync($"/api/ActivityCategory/{categoryId}");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var categoriesAfter = await client.GetFromJsonAsync<List<ActivityCategoryDTO>>("/api/ActivityCategory");
            categoriesAfter.Should().NotContain(c => c.ActivityCategoryID == categoryId);
        }
    }
}