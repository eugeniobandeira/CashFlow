using CashFlow.Application.UseCases.Expenses;
using CashFlow.Domain.Enums;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Test.Expenses.Register
{
    public class RegisterExpenseValidatorTests
    {
        [Fact]
        public void Success()
        {
            //Assert
            var validator = new ExpenseValidator();
            var req = InsertExpenseRequestBuilder.Build();
        
            //Act
            var result = validator.Validate(req);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        public void Error_Title_Empty(string title) 
        {
            //Assert
            var validator = new ExpenseValidator();
            var req = InsertExpenseRequestBuilder.Build();
            req.Title = title;

            //Act
            var result = validator.Validate(req);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors
                .Should()
                .ContainSingle()
                .And
                .Contain(e => e.ErrorMessage
                .Equals(ErrorMessageResource.TITLE_REQUIRED));
        }

        [Fact]
        public void Error_Date_Future()
        {
            //Assert
            var validator = new ExpenseValidator();
            var req = InsertExpenseRequestBuilder.Build();
            req.Date = DateTime.UtcNow.AddDays(1);

            //Act
            var result = validator.Validate(req);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors
                .Should()
                .ContainSingle()
                .And
                .Contain(e => e.ErrorMessage
                .Equals(ErrorMessageResource.EXPENSE_DATA_INVALID));
        }

        [Fact]
        public void Error_Payment_Type_Invalid()
        {
            //Assert
            var validator = new ExpenseValidator();
            var req = InsertExpenseRequestBuilder.Build();
            req.PaymentType = (PaymentTypeEnum)9999;

            //Act
            var result = validator.Validate(req);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors
                .Should()
                .ContainSingle()
                .And
                .Contain(e => e.ErrorMessage
                .Equals(ErrorMessageResource.PAYMENT_TYPE_INVALID));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Error_Amount_Invalid(decimal amount)
        {
            //Assert
            var validator = new ExpenseValidator();
            var req = InsertExpenseRequestBuilder.Build();
            req.Amount = amount;

            //Act
            var result = validator.Validate(req);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors
                .Should()
                .ContainSingle()
                .And
                .Contain(e => e.ErrorMessage
                .Equals(ErrorMessageResource.AMOUNT_MUST_BE_GREATER_THAN_ZERO));
        }
    }
}
