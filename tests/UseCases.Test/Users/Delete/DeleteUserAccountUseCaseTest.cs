using CashFlow.Application.UseCases.Users.Delete;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Users;
using FluentAssertions;

namespace UseCases.Test.Users.Delete
{
    public class DeleteUserAccountUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var user = UserBuilder.Build();
            var useCase = CreateUseCase(user);

            //Act
            var act = async () => await useCase.Execute();

            //Assert
            await act.Should().NotThrowAsync();
        }

        private static DeleteUserAccountUseCase CreateUseCase(UserEntity user)
        {
            var repository = new UserDeleteOnlyRepositoryBuilder().Build();
            var loggedUser = LoggedUserBuilder.Build(user);
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new DeleteUserAccountUseCase(unitOfWork, loggedUser, repository);
        }
    }
}
