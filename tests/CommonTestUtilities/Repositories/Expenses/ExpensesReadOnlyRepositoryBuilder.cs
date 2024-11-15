using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface.Expenses;
using DocumentFormat.OpenXml.Spreadsheet;
using Moq;
using PdfSharp.Drawing;

namespace CommonTestUtilities.Repositories.Expenses
{
    public class ExpensesReadOnlyRepositoryBuilder
    {
        private readonly Mock<IExpensesReadOnlyRepository> _repository;

        public ExpensesReadOnlyRepositoryBuilder()
        {
            _repository = new Mock<IExpensesReadOnlyRepository>();
        }

        public ExpensesReadOnlyRepositoryBuilder GetAll(UserEntity user, List<ExpenseEntity> expenses)
        {
            _repository.Setup(repo => repo.GetAllAsync(user)).ReturnsAsync(expenses);

            return this;
        }

        public ExpensesReadOnlyRepositoryBuilder GetById(UserEntity user, ExpenseEntity? expense)
        {
            if (expense is not null)
                _repository.Setup(repo => repo.GetByIdAsync(user, expense.Id)).ReturnsAsync(expense);

            return this;
        }

        public ExpensesReadOnlyRepositoryBuilder FilterByMonth(UserEntity user, List<ExpenseEntity> expenses)
        {
            _repository.Setup(repo => repo.FilterByMonth(user, It.IsAny<DateOnly>())).ReturnsAsync(expenses);

            return this;
        }

        public IExpensesReadOnlyRepository Build()
            => _repository.Object;
    }
}
