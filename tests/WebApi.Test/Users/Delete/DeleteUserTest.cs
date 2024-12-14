using FluentAssertions;
using System.Net;

namespace WebApi.Test.Users.Delete
{
    public class DeleteUserTest : CashflowClassFixture
    {
        private const string METHOD = "v1/api/user";
        private readonly string _token;
        private readonly long _userId;

        public DeleteUserTest(IntegrationTestWebApplicationFactory webAppFactory) : base(webAppFactory)
        {
            _token = webAppFactory.Regular_User_Manager.GetToken();
            _userId = webAppFactory.Regular_User_Manager.GetUserId();
        }

        [Fact]
        public async Task Success()
        {
            //Act
            var result = await DoDeleteAsync(reqUri: METHOD, token: _token);

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);

            result = await DoGetAsync(reqUri: $"{METHOD}/{_userId}", token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
