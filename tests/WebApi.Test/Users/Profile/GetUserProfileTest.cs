using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Users.Profile
{
    public class GetUserProfileTest : CashflowClassFixture
    {
        private const string METHOD = "v1/api/user";

        private readonly string _token;
        private readonly string _userName;
        private readonly string _userEmail;

        public GetUserProfileTest(IntegrationTestWebApplicationFactory webAppFactory) : base(webAppFactory)
        {
            _token = webAppFactory.Regular_User_Manager.GetToken();
            _userName = webAppFactory.Regular_User_Manager.GetName();
            _userEmail = webAppFactory.Regular_User_Manager.GetEmail();
        }

        [Fact]
        public async Task Success()
        {
            //Act
            var result = await DoGetAsync(reqUri: METHOD, token: _token);

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);

            response.RootElement.GetProperty("name").GetString().Should().Be(_userName);
            response.RootElement.GetProperty("email").GetString().Should().Be(_userEmail);
        }

    }
}
