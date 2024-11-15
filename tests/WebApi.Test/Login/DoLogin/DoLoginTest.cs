using CashFlow.Domain.Requests.Login;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin
{
    public class DoLoginTest : CashflowClassFixture
    {
        private const string METHOD = "v1/api/login";

        private readonly string _email;
        private readonly string _name;
        private readonly string _password;

        public DoLoginTest(IntegrationTestWebApplicationFactory webAppFactory) : base(webAppFactory)
        {
            _email = webAppFactory.Regular_User_Manager.GetEmail();
            _name = webAppFactory.Regular_User_Manager.GetName();
            _password = webAppFactory.Regular_User_Manager.GetPassword();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var req = new LoginRequest
            {
                Email = _email,
                Password = _password
            };

            //Act
            var response = await DoPostAsync(reqUri: METHOD, req: req);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement
                .GetProperty("name")
                .GetString()
                .Should()
                .Be(_name);

            responseData.RootElement
                .GetProperty("token")
                .GetString()
                .Should()
                .NotBeNullOrEmpty();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Invalid_Login_Error(string culture)
        {
            //Arrange
            var req = InsertLoginRequestBuilder.Build();

            //Act
            var response = await DoPostAsync(
                reqUri: METHOD, 
                req: req,
                culture: culture);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData
                .RootElement
                .GetProperty("errorMessage")
                .EnumerateArray();

            var expectedMessage = ErrorMessageResource
                .ResourceManager
                .GetString("INVALID_EMAIL_OR_PASSWORD", new CultureInfo(culture));

            errors
                .Should()
                .HaveCount(1)
                .And
                .Contain(error => error.GetString()!
                .Equals(expectedMessage));
        }
    }
}
