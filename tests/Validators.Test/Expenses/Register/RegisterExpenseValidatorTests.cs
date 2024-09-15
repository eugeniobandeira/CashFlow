using CashFlow.Application.UseCases.Expenses.Register;
using CommonTestUtilities;
using FluentAssertions;

namespace Validators.Test.Expenses.Register
{
    public class RegisterExpenseValidatorTests
    {
        [Fact]
        public void Success()
        {
            //Assert
            var validator = new RegisterExpenseValidator();
            var req = InsertExpenseRequestBuilder.Build();
        
            //Act
            var result = validator.Validate(req);

            //Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
