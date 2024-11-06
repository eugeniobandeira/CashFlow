using CashFlow.Application.UseCases.Users.Register;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Users.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var req = InsertUserRequestBuilder.Build();
            var useCase = CreateUseCase();

            //Act
            var result = await useCase.Execute(req);

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(req.Name);
            result.Token.Should().NotBeNullOrEmpty();
        }

        #region UseCase Builder
        private static RegisterUserUseCase CreateUseCase()
        {
            var mapper = MapperBuilder.Build();
            var passwordEncripter = PasswordEncripterBuilder.Build();
            var readOnlyRepository = new UserReadOnlyRepositoryBuilder().Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
            var tokenGenerator = JwtTokenGeneratorBuilder.Build();

            return new RegisterUserUseCase(mapper, passwordEncripter, readOnlyRepository, unitOfWork, writeOnlyRepository, tokenGenerator);
        }
        #endregion
    }
}
