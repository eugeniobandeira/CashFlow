using CashFlow.Application.UseCases.Users;
using CashFlow.Domain.Requests.Users;
using FluentAssertions;
using FluentValidation;

namespace Validators.Test.User
{
    public class PasswordValidatorTest
    {
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        [InlineData("a")]
        [InlineData("aa")]
        [InlineData("aaa")]
        [InlineData("aaaa")]
        [InlineData("aaaaa")]
        [InlineData("aaaaaa")]
        [InlineData("aaaaaaa")]
        [InlineData("aaaaaaaa")]
        [InlineData("Aaaaaaaa")]
        [InlineData("Aaaaaaa1")]
        public void Error_Invalid_Password(string password)
        {
            //Arrange
            var validator = new PasswordValidator<UserRequest>();

            //Act
            var result = validator.IsValid(new ValidationContext<UserRequest>(
                new UserRequest()), password);

            //Assert
            result.Should().BeFalse();
        }
    }
}
