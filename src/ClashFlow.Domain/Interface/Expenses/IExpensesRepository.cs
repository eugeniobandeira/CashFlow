using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Interface.Expenses
{
    public interface IExpensesRepository
    {
        Task Add(ExpenseEntity expense);
    }
}
