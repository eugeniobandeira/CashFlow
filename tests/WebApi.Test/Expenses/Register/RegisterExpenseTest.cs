﻿using CashFlow.Exception;
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
    public class RegisterExpenseTest : CashflowClassFixture
    {
        private const string METHOD = "v1/api/expenses";
        private readonly string _token;

        public RegisterExpenseTest(IntegrationTestWebApplicationFactory webAppFactory) : base(webAppFactory)
        {
            _token = webAppFactory.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var req = InsertExpenseRequestBuilder.Build();

            //Act
            var response = await DoPostAsync(
                reqUri: METHOD, 
                req: req,
                token: _token);

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
        public async Task Empty_Title_Error(string culture)
        {
            //Arrange
            var req = InsertExpenseRequestBuilder.Build();
            req.Title = string.Empty;

            //Act
            var response = await DoPostAsync(
                reqUri: METHOD,
                req: req,
                token: _token,
                culture: culture);

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
                .GetString("TITLE_REQUIRED", new CultureInfo(culture));

            errors
                .Should()
                .HaveCount(1)
                .And
                .Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
