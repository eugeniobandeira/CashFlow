using CashFlow.Application.UseCases.Users.ChangePassword;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Test.User.ChangePassword
{
    public class ChangePasswordValidatorTest
    {
        [Fact]
        public void Success()
        {
            //Arrange
            var validator = new ChangePasswordValidator();
            var req = ChangePasswordRequestBuilder.Build();

            //Act
            var result = validator.Validate(req);   

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        public void NewPassword_Empty_Error(string newPassword)
        {
            //Arrange
            var validator = new ChangePasswordValidator();
            var req = ChangePasswordRequestBuilder.Build();
            req.NewPassword = newPassword;

            //Act
            var result = validator.Validate(req);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(err => err.ErrorMessage.Equals(ErrorMessageResource.INVALID_PASSWORD_NULL_OR_WHITE_SPACE));
        }
    }
}
