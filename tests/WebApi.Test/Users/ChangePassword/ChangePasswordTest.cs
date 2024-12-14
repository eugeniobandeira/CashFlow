using CashFlow.Domain.Requests.Login;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.ChangePassword
{
    public class ChangePasswordTest : CashflowClassFixture
    {
        private const string METHOD = "v1/api/user/change-password";

        private readonly string _token;
        private readonly string _password;
        private readonly string _email;

        public ChangePasswordTest(IntegrationTestWebApplicationFactory webAppFactory) : base(webAppFactory)
        {
            _token = webAppFactory.Regular_User_Manager.GetToken();
            _password = webAppFactory.Regular_User_Manager.GetPassword();
            _email = webAppFactory.Regular_User_Manager.GetEmail();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange 
            var req = ChangePasswordRequestBuilder.Build();
            req.Password = _password;

            //Act
            var response = await DoPutAsync(reqUri: METHOD, req: req, token: _token);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var loginRequest = new LoginRequest
            {
                Email = _email,
                Password = _password
            };

            response = await DoPostAsync(reqUri: "v1/api/login", req: loginRequest);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            loginRequest.Password = req.NewPassword;

            response = await DoPostAsync(reqUri: "v1/api/login", req: loginRequest);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Password_Different_Current_Password_Error(string culture)
        {
            //Arrange 
            var req = ChangePasswordRequestBuilder.Build();

            //Act
            var response = await DoPutAsync(reqUri: METHOD, req: req, token: _token, culture: culture);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();  

            var respondeData = await JsonDocument.ParseAsync(responseBody);

            var errors = respondeData.RootElement.GetProperty("errorMessage").EnumerateArray();

            var expectedMessage = ErrorMessageResource.ResourceManager.GetString("PASSWORD_DIFFERNT_CURRENT_PASSWORD", new CultureInfo(culture));

            errors.Should().HaveCount(1).And.Contain(err => err.GetString()!.Equals(expectedMessage));  
        }
    }
}
