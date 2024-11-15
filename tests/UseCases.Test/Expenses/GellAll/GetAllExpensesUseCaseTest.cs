using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories.Expenses;
using FluentAssertions;

namespace WebApi.Test.Expenses.GetAll
{
    public class GetAllExpensesUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();
            var expenses = ExpenseBuilder.Collection(loggedUser);

            //Act
            var useCase = CreateUsecase(loggedUser, expenses);
            var result = await useCase.Execute();

            //Assert
            result.Should().NotBeNull();
            result
                .Expenses
                .Should()
                .NotBeNullOrEmpty()
                .And
                .AllSatisfy(exp =>
                {
                    exp.Id.Should().BeGreaterThan(0);
                    exp.Title.Should().NotBeNullOrEmpty();
                    exp.Amount.Should().BeGreaterThan(0);
                });
        }

        private static GetAllExpenseUseCase CreateUsecase(UserEntity user, List<ExpenseEntity> expenses)
        {
            var repository = new ExpensesReadOnlyRepositoryBuilder().GetAll(user, expenses).Build();
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetAllExpenseUseCase(repository, mapper, loggedUser);
        }
    }
}
