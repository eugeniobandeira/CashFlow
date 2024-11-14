using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.Register
{
    public class RegisterExpenseTest : IClassFixture<IntegrationTestWebApplicationFactory>
    {
        private const string METHOD = "v1/api/expenses";

        private readonly HttpClient _httpClient;
        private readonly string _token;

        public RegisterExpenseTest(IntegrationTestWebApplicationFactory webAppFactory)
        {
            _httpClient = webAppFactory.CreateClient();
            _token = webAppFactory.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var req = InsertExpenseRequestBuilder.Build();
            _httpClient
                .DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue("Bearer", _token);

            //Act
            var response = await _httpClient.PostAsJsonAsync(METHOD, req);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement
                .GetProperty("title")
                .GetString()
                .Should()
                .Be(req.Title);

            responseData.RootElement
                .GetProperty("description")
                .GetString()
                .Should()
                .Be(req.Description);

            responseData.RootElement
                .GetProperty("amount")
                .GetDecimal()
                .Should()
                .Be(req.Amount);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Empty_Title_Error(string cultureInfo)
        {
            //Arrange
            var req = InsertExpenseRequestBuilder.Build();
            req.Title = string.Empty;

            _httpClient
                .DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue("Bearer", _token);

            _httpClient
                .DefaultRequestHeaders
                .AcceptLanguage
                .Add(new StringWithQualityHeaderValue(cultureInfo));

            //Act
            var response = await _httpClient.PostAsJsonAsync(METHOD, req);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData
                .RootElement
                .GetProperty("errorMessage")
                .EnumerateArray();

            var expectedMessage = ErrorMessageResource
                .ResourceManager
                .GetString("TITLE_REQUIRED", new CultureInfo(cultureInfo));

            errors
                .Should()
                .HaveCount(1)
                .And
                .Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
