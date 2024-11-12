using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Interface.Expenses
{
    public interface IExpenseUpdateOnlyRepository
    {
        void Update(ExpenseEntity entity);
        Task<ExpenseEntity?> GetByIdAsync(UserEntity userEntity, long id);
    }
}
