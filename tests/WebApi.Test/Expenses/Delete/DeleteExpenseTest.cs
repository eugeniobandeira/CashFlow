using CashFlow.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.Delete
{
    public class DeleteExpenseTest : CashflowClassFixture
    {
        private const string METHOD = "v1/api/expenses";
        private readonly string _token;
        private readonly long _expenseId;

        public DeleteExpenseTest(IntegrationTestWebApplicationFactory webAppFactory)
            :base(webAppFactory)
        {
            _token = webAppFactory.Regular_User_Manager.GetToken();
            _expenseId = webAppFactory.Expense_Manager.GetExpenseId();
        }

        [Fact]
        public async Task Success()
        {
            //Act
            var result = await DoDeleteAsync(reqUri: $"{METHOD}/{_expenseId}", token: _token);

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);

            result = await DoGetAsync(reqUri: $"{METHOD}/{_expenseId}", token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Expense_Not_Found_Error(string culture)
        {
            //Act
            var result = await DoDeleteAsync(reqUri: $"{METHOD}/1000", token: _token, culture: culture);

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessage").EnumerateArray();

            var expectedMessage = ErrorMessageResource
                .ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));

            errors
                .Should()
                .HaveCount(1)
                .And
                .Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
