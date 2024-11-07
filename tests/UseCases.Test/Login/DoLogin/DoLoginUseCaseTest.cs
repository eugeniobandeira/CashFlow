using CashFlow.Application.UseCases.Login;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Requests.Login;
using CashFlow.Domain.Responses.Users;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Login.DoLogin
{
    public class DoLoginUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var user = UserBuilder.Build();
            var req = InsertLoginRequestBuilder.Build();
            req.Email = user.Email;

            //Act
            var useCase = CreateUseCase(user, req.Password);
            var result = await useCase.Execute(req);

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(user.Name);
            result.Token.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task User_Not_Found_Error()
        {
            //Arrange
            var user = UserBuilder.Build();
            var req = InsertLoginRequestBuilder.Build();

            //Act
            var useCase = CreateUseCase(user, req.Password);
            var act = async () => await useCase.Execute(req);

            //Assert
            var result = await act.Should().ThrowAsync<InvalidLoginException>();
            result.Where(ex => ex.GetErrors().Count == 1 &&
            ex.GetErrors().Contains(ErrorMessageResource.INVALID_EMAIL_OR_PASSWORD));
        }

        [Fact]
        public async Task Password_Does_Not_Match_Error()
        {
            //Arrange
            var user = UserBuilder.Build();
            var req = InsertLoginRequestBuilder.Build();
            req.Email = user.Email;

            //Act
            var useCase = CreateUseCase(user);
            var act = async () => await useCase.Execute(req);

            //Assert
            var result = await act.Should().ThrowAsync<InvalidLoginException>();
            result.Where(ex => ex.GetErrors().Count == 1 &&
            ex.GetErrors().Contains(ErrorMessageResource.INVALID_EMAIL_OR_PASSWORD));

        }

        #region Create UseCase
        private static DoLoginUseCase CreateUseCase(UserEntity userEntity, string? password = null)
        {
            var passwordEncripter = new PasswordEncrypterBuilder().Verify(password).Build();
            var tokenGenerator = JwtTokenGeneratorBuilder.Build();
            var readOnlyRepository = new UserReadOnlyRepositoryBuilder().GetUserByEmail(userEntity).Build();

            return new DoLoginUseCase(readOnlyRepository, passwordEncripter, tokenGenerator);
        }
        #endregion
    }
}
