using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using WeekMap;
using WeekMap.DTOs;

public static class UserTestHelper
{
    static private readonly HttpClient _client;
    static private readonly WebApplicationFactory<Program> _factory;

    static private readonly UserTestData _userTestData = new();
    static private readonly UserDTO sampleUser = new UserDTO { Username = "sampleUsernameASDFj253", Email = "sampleemail@dasfasfaf.com", Password = "SamplePassword245325" };

    public static async Task RegisterUser(HttpClient _client)
    {
        var registerResponse = await _client.PostAsJsonAsync("api/register", sampleUser);
        registerResponse.EnsureSuccessStatusCode();
    }

    public static async Task LoginUser(HttpClient _client)
    {
        var loginResponse = await _client.PostAsJsonAsync("api/login", new UserDTO { Username = sampleUser.Username, Password = sampleUser.Password });
        loginResponse.EnsureSuccessStatusCode();
    }
}

