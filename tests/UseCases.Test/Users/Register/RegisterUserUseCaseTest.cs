using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
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

        [Fact]
        public async Task Empty_Name_Error()
        {
            //Arrange
            var req = InsertUserRequestBuilder.Build();
            req.Name = string.Empty;

            var useCase = CreateUseCase();

            //Act
            var act = async () => await useCase.Execute(req);

            //Assert
            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

            result.Where(ex => ex.GetErrors().Count == 1 
            && ex.GetErrors().Contains(ErrorMessageResource.EMPTY_NAME));
        }

        [Fact]
        public async Task Email_Already_Registered_Error()
        {
            //Arrange
            var req = InsertUserRequestBuilder.Build();

            var useCase = CreateUseCase(req.Email);

            //Act
            var act = async () => await useCase.Execute(req);

            //Assert
            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

            result.Where(ex => ex.GetErrors().Count == 1
            && ex.GetErrors().Contains(ErrorMessageResource.EMAIL_ALREADY_REGISTERED));
        }

        #region UseCase Builder
        private static RegisterUserUseCase CreateUseCase(string? email = null)
        {
            var mapper = MapperBuilder.Build();
            var passwordEncripter = PasswordEncripterBuilder.Build();
            var readOnlyRepository = new UserReadOnlyRepositoryBuilder();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
            var tokenGenerator = JwtTokenGeneratorBuilder.Build();

            if (string.IsNullOrEmpty(email) is false)
                readOnlyRepository.ExistUserWithEmail(email);

            return new RegisterUserUseCase(mapper, passwordEncripter, readOnlyRepository.Build(), unitOfWork, writeOnlyRepository, tokenGenerator);
        }
        #endregion
    }
}
