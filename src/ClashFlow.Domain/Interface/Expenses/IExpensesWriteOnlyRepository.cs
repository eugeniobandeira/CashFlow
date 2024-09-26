using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Interface.Expenses
{
    public interface IExpensesWriteOnlyRepository
    {
        Task AddAsync(ExpenseEntity expense);
    }
}
