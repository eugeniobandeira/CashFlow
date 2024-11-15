using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface.Expenses;
using Moq;

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

        public IExpensesReadOnlyRepository Build()
            => _repository.Object;
    }
}
