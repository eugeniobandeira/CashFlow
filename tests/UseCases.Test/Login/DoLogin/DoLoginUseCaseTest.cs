using CashFlow.Application.UseCases.Login;
using CashFlow.Domain.Entities;
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
            var useCase = CreateUseCase(user);

            //Act
            var result = await useCase.Execute(req);

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(user.Name);
            result.Token.Should().NotBeNullOrWhiteSpace();
        }

        #region Create UseCase
        private static DoLoginUseCase CreateUseCase(UserEntity userEntity)
        {
            var passwordEncripter = PasswordEncripterBuilder.Build();
            var tokenGenerator = JwtTokenGeneratorBuilder.Build();
            var readOnlyRepository = new UserReadOnlyRepositoryBuilder().GetUserByEmail(userEntity).Build();

            return new DoLoginUseCase(readOnlyRepository, passwordEncripter, tokenGenerator);
        }
        #endregion
    }
}
