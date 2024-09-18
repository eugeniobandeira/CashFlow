using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Interface.Expenses
{
    public interface IExpensesRepository
    {
        void Add(ExpenseEntity expense);
    }
}
