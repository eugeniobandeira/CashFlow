using FluentAssertions;
using System.Net;
using System.Net.Mime;

namespace WebApi.Test.Expenses.Reports
{
    public class GenerateExpenseReportTest : CashflowClassFixture
    {
        private const string METHOD = "v1/api/report";
        private readonly string _adminToken;
        private readonly DateTime _expenseDate;

        public GenerateExpenseReportTest(IntegrationTestWebApplicationFactory webAppFactory) : base(webAppFactory)
        {
            _adminToken = webAppFactory.Admin_User_Manager.GetToken();
            _expenseDate = webAppFactory.Admin_User_Expense_Manager.GetDate();
        }

        [Fact]
        public async Task Success_Pdf()
        {
            //Act
            var result = await DoGetAsync(
                reqUri: $"{METHOD}/pdf?year={_expenseDate.Year}&month={_expenseDate.Month}",
                token: _adminToken);

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Content.Headers.ContentType.Should().NotBeNull();
            result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Pdf);
        }

        [Fact]
        public async Task Success_Excel()
        {
            //Act
            var result = await DoGetAsync(
                reqUri: $"{METHOD}/excel?year={_expenseDate.Year}&month={_expenseDate.Month}",
                token: _adminToken);

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Content.Headers.ContentType.Should().NotBeNull();
            result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Octet);
        }
    }
}
