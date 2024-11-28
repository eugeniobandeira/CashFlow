using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Test.User.Update
{
    public class UpdateUserValidatorTest
    {
        [Fact]
        public void Success()
        {
            //Arrange
            var valiidator = new UpdateUserValidator();
            var req = UpdateUserRequestBuilder.Build();

            //Act
            var result = valiidator.Validate(req);

            //Assert
            result.IsValid.Should().BeTrue();  
        }

        [Theory]
        [InlineData("")]
        [InlineData("      ")]
        [InlineData(null)]
        public void Empty_Name_Error(string name)
        {
            //Arrange
            var valiidator = new UpdateUserValidator();
            var req = UpdateUserRequestBuilder.Build();
            req.Name = name;

            //Act
            var result = valiidator.Validate(req);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ErrorMessageResource.EMPTY_NAME));
        }

        [Theory]
        [InlineData("")]
        [InlineData("      ")]
        [InlineData(null)]
        public void Empty_Email_Error(string email)
        {
            //Arrange
            var valiidator = new UpdateUserValidator();
            var req = UpdateUserRequestBuilder.Build();
            req.Email = email;

            //Act
            var result = valiidator.Validate(req);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ErrorMessageResource.EMPTY_EMAIL));
        }

        [Fact]
        public void Invalid_Email_Error()
        {
            //Arrange
            var valiidator = new UpdateUserValidator();
            var req = UpdateUserRequestBuilder.Build();
            req.Email = "eugenio.com";

            //Act
            var result = valiidator.Validate(req);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ErrorMessageResource.INVALID_EMAIL));
        }
    }
}
