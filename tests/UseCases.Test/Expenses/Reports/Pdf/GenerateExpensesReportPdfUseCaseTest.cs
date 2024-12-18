using CashFlow.Application.UseCases.Expenses.Reports.Pdf;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories.Expenses;
using FluentAssertions;

namespace UseCases.Test.Expenses.Reports.Pdf
{
    public class GenerateExpensesReportPdfUseCaseTest
    {
        [Fact]
        public async Task Sucsess()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();
            var expenses = ExpenseBuilder.Collection(loggedUser);

            //Act
            var useCase = CreateUseCase(loggedUser, expenses);

            //Assert
            var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Empty_List_Sucsess()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();

            //Act
            var useCase = CreateUseCase(loggedUser, []);

            //Assert
            var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));
            result.Should().BeEmpty();
        }

        public static GenerateExpensesReportPdfUseCase CreateUseCase(UserEntity user, List<ExpenseEntity> expenses)
        {
            var repository = new ExpensesReadOnlyRepositoryBuilder().FilterByMonth(user, expenses).Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GenerateExpensesReportPdfUseCase(repository, loggedUser);
        }
    }
}
