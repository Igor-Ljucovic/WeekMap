using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WeekMap.DTOs;

public static class UserTestHelper
{
    static private readonly UserDTO sampleUser = new UserDTO { Username = "sampleUsernameASDFj253", Email = "sampleemail@dasfasfaf.com", Password = "SamplePassword245325" };

    public static async Task RegisterUser(HttpClient _client)
    {
        var registerResponse = await _client.PostAsJsonAsync("api/register", sampleUser);
        registerResponse.EnsureSuccessStatusCode();
    }

    public static async Task LoginUser(HttpClient _client)
    {
        var loginResponse = await _client.PostAsJsonAsync("api/login", sampleUser);
        loginResponse.EnsureSuccessStatusCode();
        await SetBearerToken(_client, loginResponse);
    }

    public static async Task SetBearerToken(HttpClient _client, HttpResponseMessage loginResponse)
    {
        var body = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
        var token = body.GetProperty("access_token").GetString();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}

