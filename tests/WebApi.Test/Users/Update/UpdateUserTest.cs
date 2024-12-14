using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Update
{
    public class UpdateUserTest : CashflowClassFixture
    {
        private const string METHOD = "v1/api/user/profile";
        private readonly string _token;

        public UpdateUserTest(IntegrationTestWebApplicationFactory webAppFactory) : base(webAppFactory)
        {
            _token = webAppFactory.Regular_User_Manager.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var req = UpdateUserRequestBuilder.Build();

            //Act
            var response = await DoPutAsync(reqUri: METHOD, req: req, token: _token);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Empty_Name_Error(string culture)
        {
            //Arrange
            var req = UpdateUserRequestBuilder.Build();
            req.Name = string.Empty;

            //Act
            var response = await DoPutAsync(METHOD, req, token: _token, culture: culture);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errorMessage").EnumerateArray();

            var expectedMessage = ErrorMessageResource.ResourceManager.GetString("EMPTY_NAME", new CultureInfo(culture));

            errors.Should().HaveCount(1).And.Contain(err => err.GetString()!.Equals(expectedMessage));
        }

    }
}
