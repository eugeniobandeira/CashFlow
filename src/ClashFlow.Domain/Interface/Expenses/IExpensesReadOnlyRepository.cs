using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Interface.Expenses
{
    public interface IExpensesReadOnlyRepository
    {
        Task<List<ExpenseEntity>> GetAllAsync();
        Task<ExpenseEntity?> GetByIdAsync(long id);
        Task<List<ExpenseEntity>> FilterByMonth(DateOnly month);
    }
}
