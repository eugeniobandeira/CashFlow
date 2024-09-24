using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Interface.Expenses
{
    public interface IExpensesRepository
    {
        Task AddAsync(ExpenseEntity expense);
        Task<List<ExpenseEntity>> GetAllAsync();
        Task<ExpenseEntity?> GetExpensebyIdAsync(long id);
    }
}
