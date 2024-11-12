using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Interface.Expenses
{
    public interface IExpensesReadOnlyRepository
    {
        Task<List<ExpenseEntity>> GetAllAsync(UserEntity userEntity);
        Task<ExpenseEntity?> GetByIdAsync(UserEntity userEntity, long id);
        Task<List<ExpenseEntity>> FilterByMonth(DateOnly month);
    }
}
