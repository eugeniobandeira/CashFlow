using CommonTestUtilities.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace WebApi.Test.Users.Register
{
    public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
    {
        private const string METHOD = "v1/api/user";

        private readonly HttpClient _httpClient;

        public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory)
        {
            _httpClient = webApplicationFactory.CreateClient();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var req = InsertUserRequestBuilder.Build();

            //Act
            var result = await _httpClient.PostAsJsonAsync(METHOD, req);

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}
