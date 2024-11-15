using CashFlow.Domain.Enums;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Expenses.GetById
{
    public class GetExpenseByIdTest : CashflowClassFixture
    {
        private const string METHOD = "v1/api/expenses";
        private readonly string _token;
        private readonly long _expenseId;

        public GetExpenseByIdTest(IntegrationTestWebApplicationFactory webAppFactory)
            : base(webAppFactory)
        {
            _token = webAppFactory.GetToken();
            _expenseId = webAppFactory.GetExpenseId();
        }

        [Fact]
        public async Task Success()
        {
            //Act
            var result = await DoGetAsync(reqUri: $"{METHOD}/{_expenseId}", token: _token);

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            response.RootElement.GetProperty("id").GetInt64().Should().Be(_expenseId);                       
            response.RootElement.GetProperty("title").GetString().Should().NotBeNullOrWhiteSpace();
            response.RootElement.GetProperty("description").GetString().Should().NotBeNullOrWhiteSpace();
            response.RootElement.GetProperty("date").GetDateTime().Should().NotBeAfter(DateTime.Today);
            response.RootElement.GetProperty("amount").GetDecimal().Should().BeGreaterThan(0);

            var paymentType = response.RootElement.GetProperty("paymentType").GetInt32();
            Enum.IsDefined(typeof(PaymentTypeEnum), paymentType).Should().BeTrue();
        }
    }
}
