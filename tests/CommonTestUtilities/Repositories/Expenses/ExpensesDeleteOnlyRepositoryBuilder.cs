using CashFlow.Domain.Interface.Expenses;
using Moq;

namespace CommonTestUtilities.Repositories.Expenses
{
    public class ExpensesDeleteOnlyRepositoryBuilder
    {
        private readonly Mock<IExpenseDeleteOnlyRepository> _repository;

        public ExpensesDeleteOnlyRepositoryBuilder()
        {
            _repository = new Mock<IExpenseDeleteOnlyRepository>();
        }

        public ExpensesDeleteOnlyRepositoryBuilder DoDeleteAsync(long expenseId)
        {
            _repository
                .Setup(repo => repo.DeleteAsync(expenseId))
                .Returns(Task.CompletedTask);

            return this;
        }

        public IExpenseDeleteOnlyRepository Build()
            => _repository.Object;
    }
}
