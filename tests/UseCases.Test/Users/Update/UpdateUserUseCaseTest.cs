using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.User;
using CommonTestUtilities.Repositories.Users;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Users.Update
{
    public class UpdateUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var user = UserBuilder.Build();
            var req = UpdateUserRequestBuilder.Build();

            //Act
            var useCase = CreateUseCase(user);
            var act = async () => await useCase.Execute(req);

            //Assert
            await act.Should().NotThrowAsync();
            user.Name.Should().Be(req.Name);
            user.Email.Should().Be(req.Email);
        }

        [Fact]
        public async Task Empty_Name_Error()
        {
            //Arrange
            var user = UserBuilder.Build();
            var req = UpdateUserRequestBuilder.Build();
            req.Name = string.Empty;

            //Act
            var useCase = CreateUseCase(user);
            var act = async () => await useCase.Execute(req);

            //Assert
            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ErrorMessageResource.EMPTY_NAME));
        }

        [Fact]
        public async Task Email_Already_Exist_Error()
        {
            //Arrange
            var user = UserBuilder.Build();
            var req = UpdateUserRequestBuilder.Build();

            //Act
            var useCase = CreateUseCase(user, req.Email);
            var act = async () => await useCase.Execute(req);

            //Assert
            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ErrorMessageResource.EMAIL_ALREADY_REGISTERED));
        }

        private static UpdateUserUseCase CreateUseCase(UserEntity user, string? email = null)
        {
            var unitOfWork = UnitOfWorkBuilder.Build();
            var updateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
            var loggedUser = LoggedUserBuilder.Build(user);
            var readRepository = new UserReadOnlyRepositoryBuilder().Build();

            if (!string.IsNullOrWhiteSpace(email))
                readRepository.ExistUserWithEmail(email);

            return new UpdateUserUseCase(loggedUser, updateRepository, readRepository, unitOfWork);
        }
    }
}
