using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Expenses;
using FluentAssertions;

namespace UseCases.Test.Expenses.Delete
{
    public class DeleteExpenseUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();
            var expense = ExpenseBuilder.Build(loggedUser);

            //Act
            var useCase = CreateUseCase(loggedUser, expense);
            var act = async () => await useCase.Execute(expense.Id);

            //Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Not_Found_Expense_Error()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();

            //Act
            var useCase = CreateUseCase(loggedUser);
            var act = async () => await useCase.Execute(id: 1000);
            var result = await act.Should().ThrowAsync<NotFoundException>();

            //Assert
            result
                .Where(ex =>  ex.GetErrors().Count == 1 &&  ex.GetErrors()
                .Contains(ErrorMessageResource.EXPENSE_NOT_FOUND));
        }

        private static DeleteExpenseUseCase CreateUseCase(UserEntity user, ExpenseEntity? expense = null)
        {
            var repositoryDelete = new ExpensesDeleteOnlyRepositoryBuilder()
                .DoDeleteAsync(expense?.Id ?? 0)
                .Build();

            var repositoryReadOnly = new ExpensesReadOnlyRepositoryBuilder()
                .GetById(user, expense)
                .Build();

            var unitOfWork = UnitOfWorkBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new DeleteExpenseUseCase(repositoryDelete, repositoryReadOnly, unitOfWork, loggedUser);
        }

    }
}
