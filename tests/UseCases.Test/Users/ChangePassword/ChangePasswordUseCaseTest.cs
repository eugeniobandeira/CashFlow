using CashFlow.Application.UseCases.Users.ChangePassword;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Users;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Users.ChangePassword
{
    public class ChangePasswordUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var user = UserBuilder.Build();
            var req = ChangePasswordRequestBuilder.Build();

            //Act
            var useCase = CreateUseCase(user, req.Password);
            var act = async () => await useCase.Execute(req);

            //Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task NewPassword_Empty_Error()
        {
            //Arrange
            var user = UserBuilder.Build();
            var req = ChangePasswordRequestBuilder.Build();
            req.NewPassword = string.Empty;

            //Act
            var useCase = CreateUseCase(user, req.Password);
            var act = async () => { await useCase.Execute(req); };

            //Assert
            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
            result.Where(error => error.GetErrors().Count == 1 &&
                error.GetErrors().Contains(ErrorMessageResource.INVALID_PASSWORD_NULL_OR_WHITE_SPACE));
        }


        [Fact]
        public async Task CurrentPassword_Different_Error()
        {
            //Arrange
            var user = UserBuilder.Build();
            var req = ChangePasswordRequestBuilder.Build();

            //Act
            var useCase = CreateUseCase(user);
            var act = async () => { await useCase.Execute(req); };

            //Assert
            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
            result.Where(error => error.GetErrors().Count == 1 &&
                error.GetErrors().Contains(ErrorMessageResource.PASSWORD_DIFFERNT_CURRENT_PASSWORD));
        }

        private static ChangePasswordUseCase CreateUseCase(UserEntity user, string? password = null)
        {
            var unitOfWork = UnitOfWorkBuilder.Build();
            var userUpdateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
            var loggedUser = LoggedUserBuilder.Build(user);
            var passwordEncripter = new PasswordEncrypterBuilder().Verify(password).Build();

            return new ChangePasswordUseCase(loggedUser, passwordEncripter, userUpdateRepository, unitOfWork);
        }
    }
}
