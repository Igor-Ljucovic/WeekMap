using Microsoft.AspNetCore.Mvc.Testing;
using WebApp;

namespace XUnitTests.TestData
{
    internal static class ClientAsync
    {
        private static readonly WebApplicationFactory<Program> _factory;
        private static readonly ActivityCategoryTestData _activityCategoryTestData;

        private static WebApplicationFactory<Program> CreateFactory()
        {
            return new CustomWebApplicationFactory();
        }

        private static HttpClient CreateClient(WebApplicationFactory<Program> factory)
        {
            return factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                HandleCookies = true
            });
        }

        public static async Task<(HttpClient client, WebApplicationFactory<Program> factory)> CreateAuthenticatedClientAsync()
        {
            var factory = CreateFactory();
            var client = CreateClient(factory);

            await AccountTestHelper.RegisterUser(client);
            await AccountTestHelper.LoginUser(client);

            return (client, factory);
        }
    }
}
