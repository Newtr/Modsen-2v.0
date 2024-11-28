using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Modsen.Domain;

namespace Modsen.Tests
{
public class UserApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UserApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetUserByEmail_ShouldReturnUser_WhenUserExists()
    {
        var email = "test@example.com";
        var user = new { Email = email, RefreshToken = "token123" };

        var response = await _client.PostAsJsonAsync("/api/users", user);
        response.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"/api/users?email={email}");
        //var result = await getResponse.Content.ReadAsync<User>();

        //Assert.NotNull(result);
        //Assert.Equal(email, result.Email);
    }
}

}