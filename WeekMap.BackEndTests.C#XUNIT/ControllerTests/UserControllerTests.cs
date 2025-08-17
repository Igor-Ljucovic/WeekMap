using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;
using FluentAssertions;
using WeekMap.DTOs;
using XUnitTests.TestData;
using WeekMap;
using Microsoft.AspNetCore.Http;

namespace XUnitTests.Controllers
{
    public class UserControllerTests
    {
        private readonly UserTestData _userTestData = new UserTestData();

        [Fact]
        public async Task RegisterUser()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync(false, false);
            await using var _factory = factory;

            HttpResponseMessage? response;

            foreach (var user in _userTestData.Valid)
            {
                response = await client.PostAsJsonAsync("api/register", user);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                // can't use the get method for users since it requires authentication
            }

            foreach (var user in _userTestData.Invalid)
            {
                response = await client.PostAsJsonAsync("api/register", user);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task LoginUser()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync(false, false);
            await using var _factory = factory;

            HttpResponseMessage? response;
            List<UserDTO>? users;

            foreach (var user in _userTestData.Valid)
            {
                response = await client.PostAsJsonAsync("api/register", user);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                response = await client.PostAsJsonAsync("api/login", user);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                response = await client.GetAsync("api/users");
                users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();

                users.Should().ContainSingle(u => u.Username == user.Username);
                // Password check is typically skipped because it's hashed, but here’s how it would look:
                // users.Should().ContainSingle(u => u.Password == user.Password); // Only if passwords are stored or returned in plaintext
                users.Should().ContainSingle(u => u.Email == user.Email);
            }

            foreach (var user in _userTestData.Invalid)
            {
                response = await client.PostAsJsonAsync("api/register", user);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

                response = await client.PostAsJsonAsync("api/login", user);
                response.StatusCode.Should().Match(status => status == System.Net.HttpStatusCode.BadRequest || status == System.Net.HttpStatusCode.Unauthorized);
            }
        }

        [Fact]
        public async Task LogoutUser()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync(false, false);
            await using var _factory = factory;

            HttpResponseMessage? response;
            List<UserDTO>? users;

            foreach (var user in _userTestData.Valid)
            {
                response = await client.PostAsJsonAsync("api/register", user);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                response = await client.PostAsJsonAsync("api/login", user);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                response = await client.GetAsync("api/users");
                users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();

                users.Should().ContainSingle(u => u.Username == user.Username);
                //users.Should().ContainSingle(u => u.Password == user.Password);
                users.Should().ContainSingle(u => u.Email == user.Email);

                response = await client.PostAsJsonAsync("api/logout", user);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            }

            foreach (var user in _userTestData.Invalid)
            {
                response = await client.PostAsJsonAsync("api/register", user);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

                response = await client.PostAsJsonAsync("api/login", user);
                response.StatusCode.Should().Match(status => status == System.Net.HttpStatusCode.BadRequest || status == System.Net.HttpStatusCode.Unauthorized);

                response = await client.PostAsJsonAsync("api/logout", user);
                response.StatusCode.Should().Match(status => status == System.Net.HttpStatusCode.BadRequest || status == System.Net.HttpStatusCode.Unauthorized);
            }
        }

        [Fact]
        public async Task GetUser()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            var response = await client.GetAsync("api/users");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var user = _userTestData.Valid.ElementAt(0);
            await client.PostAsJsonAsync("api/users", user);

            var users = await client.GetFromJsonAsync<List<UserDTO>>("api/users");

            users.Should().ContainSingle(u => u.Username == user.Username);

            // passwords can't be compared because they are hashed
            //users.Should().ContainSingle(u => u.Password == user.Password);
            users.Should().ContainSingle(u => u.Email == user.Email);
        }

        [Fact]
        public async Task PostUser()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            HttpResponseMessage? response;
            List<UserDTO>? users;

            foreach (var user in _userTestData.Valid)
            {
                response = await client.PostAsJsonAsync("api/users", user);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                response = await client.GetAsync("/api/users");
                users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();

                users.Should().ContainSingle(u => u.Username == user.Username);
                // users.Should().ContainSingle(u => u.Password == user.Password);
                users.Should().ContainSingle(u => u.Email == user.Email);

                long? id = users?.First().UserID;
                response = await client.DeleteAsync($"/api/users/{id}");
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            }

            foreach (var activity in _userTestData.Invalid)
            {
                response = await client.PostAsJsonAsync("api/users", activity);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

                response = await client.GetAsync("/api/users");
                users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();
                // because 1 user needs to be logged in in order to use the method
                users.Should().HaveCount(1);
            }
        }

        [Fact]
        public async Task PutUser()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            List<UserDTO>? users;

            UserDTO userDTO = new UserDTO { Username = "user1", Email = "user@gmail.com", Password = "StrongPass1" };
            HttpResponseMessage? response = await client.PostAsJsonAsync("/api/Users", userDTO);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            users = await client.GetFromJsonAsync<List<UserDTO>>("/api/Users");
            var id = users?.First().UserID;

            foreach (var user in _userTestData.Valid)
            {
                var updatedUser = user;
                response = await client.PutAsJsonAsync($"/api/users/{id}", updatedUser);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                var afterEditUser = await client.GetFromJsonAsync<List<UserDTO>>("/api/users");
                var editedUser = afterEditUser?.FirstOrDefault(u => u.UserID == id);

                editedUser?.Username.Should().Be(updatedUser.Username);
                //editedUser?.Password.Should().Be(updatedUser.Password);
                editedUser?.Email.Should().Be(updatedUser.Email);
                users.Should().HaveCount(2);
            }

            await client.PutAsJsonAsync($"api/users/{id}", userDTO);

            foreach (var user in _userTestData.Invalid)
            {
                response = await client.PutAsJsonAsync($"api/users/{id}", user);
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

                response = await client.GetAsync("/api/users");
                users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();
                users.Should().HaveCount(2);
            }
        }

        [Fact]
        public async Task DeleteUser()
        {
            var (client, factory) = await ClientAsync.CreateAuthenticatedClientAsync();
            await using var _factory = factory;

            var user = _userTestData.Valid.ElementAt(0);
            await client.PostAsJsonAsync("api/users", user);
            var users = await client.GetFromJsonAsync<List<UserDTO>>("api/users");
            var id = users?.First().UserID;

            var response = await client.DeleteAsync($"api/users/{id}");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
