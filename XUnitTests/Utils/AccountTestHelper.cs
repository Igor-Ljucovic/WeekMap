using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApp;
using WeekMap.DTOs;

public static class AccountTestHelper
{
    static private readonly HttpClient _client;
    static private readonly WebApplicationFactory<Program> _factory;

    static private readonly UserTestData _userTestData = new();
    static private readonly RegisterDTO sampleUser = _userTestData.Valid.ElementAt(0);

    public static async Task RegisterUser(HttpClient _client)
    {
        var registerResponse = await _client.PostAsJsonAsync("api/register", sampleUser);

        registerResponse.EnsureSuccessStatusCode();
    }

    public static async Task LoginUser(HttpClient _client)
    {
        var loginResponse = await _client.PostAsJsonAsync("api/login", new LoginDTO { Username = sampleUser.Username, Password = sampleUser.Password });

        loginResponse.EnsureSuccessStatusCode();
    }
}

