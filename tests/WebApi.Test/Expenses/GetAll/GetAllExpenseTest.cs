using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Expenses.GetAll
{
    public class GetAllExpenseTest : CashflowClassFixture
    {
        private const string METHOD = "v1/api/expenses";
        private readonly string _token;

        public GetAllExpenseTest(IntegrationTestWebApplicationFactory webAppFactory)
            :base(webAppFactory)
        {
            _token = webAppFactory.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var result = await DoGetAsync(reqUri: METHOD, token: _token);

            //Action
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            //Assert
            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            response
                .RootElement
                .GetProperty("registeredExpenses")
                .EnumerateArray()
                .Should()
                .NotBeNullOrEmpty();
        }
    }
}
