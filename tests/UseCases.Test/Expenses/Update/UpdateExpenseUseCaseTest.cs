using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Expenses;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Expenses.Update
{
    public class UpdateExpenseUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();
            var expense = ExpenseBuilder.Build(loggedUser);
            var req = InsertExpenseRequestBuilder.Build();

            //Act
            var useCase = CreateUseCase(loggedUser, expense);
            var act = async () => await useCase.Execute(expense.Id, req);

            //Assert
            await act.Should().NotThrowAsync();

            expense.Title.Should().Be(req.Title);
            expense.Description.Should().Be(req.Description);
            expense.Date.Should().Be(req.Date);
            expense.Amount.Should().Be(req.Amount);
            expense.PaymentType.Should().Be((PaymentTypeEnum)req.PaymentType);
        }

        [Fact]
        public async Task Empty_Title_Error()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();
            var expense = ExpenseBuilder.Build(loggedUser);

            var req = InsertExpenseRequestBuilder.Build();
            req.Title = string.Empty;

            //Act
            var useCase = CreateUseCase(loggedUser, expense);
            var act = async () => await useCase.Execute(expense.Id, req);

            //Assert
            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
            result.Where(ex => ex.GetErrors().Count() == 1 && ex.GetErrors()
                .Contains(ErrorMessageResource.TITLE_REQUIRED));
        }


        [Fact]
        public async Task Expense_Not_Found_Error()
        {
            //Arrange
            var loggedUser = UserBuilder.Build();
            var req = InsertExpenseRequestBuilder.Build();

            //Act
            var useCase = CreateUseCase(loggedUser);
            var act = async () => await useCase.Execute(id: 10000, req);

            //Assert
            var result = await act.Should().ThrowAsync<NotFoundException>();
            result.Where(ex => ex.GetErrors().Count() == 1 && ex.GetErrors()
                .Contains(ErrorMessageResource.EXPENSE_NOT_FOUND));
        }

        #region UseCase Builder
        private static UpdateExpenseUseCase CreateUseCase(UserEntity userEntity, ExpenseEntity? expenseEntity = null)
        {
            var repository = new ExpensesUpdateOnlyRepositoryBuilder().GetById(userEntity, expenseEntity).Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(userEntity);

            return new UpdateExpenseUseCase(mapper, unitOfWork, repository, loggedUser);
        }
        #endregion
    }
}
