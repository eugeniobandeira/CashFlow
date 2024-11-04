using CashFlow.Application.UseCases.Users;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Test.User.Register
{
    public class RegisterUserValidatorTest
    {
        [Fact]
        public void Success()
        {
            //Arrange
            var validator = new UserValidator();
            var req = InsertUserRequestBuilder.Build();

            //Act
            var result = validator.Validate(req);

            //Assert
            result.IsValid.Should().BeTrue();  
        }

        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData(null)]
        public void Error_Empty_Name(string name)
        {
            //Arrange
            var validator = new UserValidator();
            var req = InsertUserRequestBuilder.Build();
            req.Name = name;

            //Act
            var result = validator.Validate(req);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors
                .Should()
                .ContainSingle()
                .And
                .Contain(error => error.ErrorMessage
                .Equals(ErrorMessageResource.EMPTY_NAME));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Error_Empty_Email(string email)
        {
            //Arrange
            var validator = new UserValidator();
            var req = InsertUserRequestBuilder.Build();
            req.Email = email;

            //Act
            var result = validator.Validate(req);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors
                .Should()
                .ContainSingle()
                .And
                .Contain(error => error.ErrorMessage
                .Equals(ErrorMessageResource.EMPTY_EMAIL));
        }

        [Fact]
        public void Error_Invalid_Email()
        {
            //Arrange
            var validator = new UserValidator();
            var req = InsertUserRequestBuilder.Build();
            req.Email = "eugenio.com";

            //Act
            var result = validator.Validate(req);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors
                .Should()
                .ContainSingle()
                .And
                .Contain(error => error.ErrorMessage
                .Equals(ErrorMessageResource.INVALID_EMAIL));
        }

        [Fact]
        public void Error_Empty_Password()
        {
            //Arrange
            var validator = new UserValidator();
            var req = InsertUserRequestBuilder.Build();
            req.Password = string.Empty;

            //Act
            var result = validator.Validate(req);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors
                .Should()
                .ContainSingle()
                .And
                .Contain(error => error.ErrorMessage
                .Equals(ErrorMessageResource.INVALID_PASSWORD_NULL_OR_WHITE_SPACE));
        }
    }
}
