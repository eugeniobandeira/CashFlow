using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Register
{
    public class RegisterUserTest : CashflowClassFixture
    {
        private const string METHOD = "v1/api/user";

        public RegisterUserTest(IntegrationTestWebApplicationFactory webAppFactory) 
            : base(webAppFactory)
        {  }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var req = InsertUserRequestBuilder.Build();

            //Act
            var result = await DoPostAsync(METHOD, req);

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.Created);

            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);

            response.RootElement
                .GetProperty("name")
                .GetString()
                .Should()
                .Be(req.Name);

            response.RootElement
                .GetProperty("token")
                .GetString()
                .Should()
                .NotBeNullOrEmpty();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Empty_Name_Error(string culture)
        {
            //Arrange
            var req = InsertUserRequestBuilder.Build();
            req.Name = string.Empty;

            //Act
            var result = await DoPostAsync(
                reqUri: METHOD, 
                req: req, 
                culture: culture);

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessage").EnumerateArray();

            var expectedMessage = ErrorMessageResource
                .ResourceManager
                .GetString("EMPTY_NAME", new CultureInfo(culture));

            errors
                .Should()
                .HaveCount(1)
                .And
                .Contain(error => error.GetString()!
                .Equals(expectedMessage));
        }
    }
}
