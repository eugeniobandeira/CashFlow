using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface.Expenses;
using Moq;

namespace CommonTestUtilities.Repositories.Expenses
{
    public class ExpensesUpdateOnlyRepositoryBuilder
    {
        private readonly Mock<IExpenseUpdateOnlyRepository> _repository;

        public ExpensesUpdateOnlyRepositoryBuilder()
        {
            _repository = new Mock<IExpenseUpdateOnlyRepository>();
        }

        public ExpensesUpdateOnlyRepositoryBuilder GetById(UserEntity user, ExpenseEntity? expense)
        {
            if (expense is not null)
                _repository.Setup(repo => 
                    repo.GetByIdAsync(user, expense!.Id)).ReturnsAsync(expense);

            return this;
        }

        public IExpenseUpdateOnlyRepository Build()
            => _repository.Object;
    }
}
