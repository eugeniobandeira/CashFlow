using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories.Expense
{
    internal class ExpensesRepository :
        IExpensesReadOnlyRepository,
        IExpensesWriteOnlyRepository,
        IExpenseDeleteOnlyRepository,
        IExpenseUpdateOnlyRepository
    {
        private readonly CashFlowDbContext _dbContext;

        public ExpensesRepository(CashFlowDbContext cashFlowDbContext)
        {
            _dbContext = cashFlowDbContext;
        }

        public async Task AddAsync(ExpenseEntity expenseEntity)
        {
            await _dbContext.Expenses.AddAsync(expenseEntity);
        }

        public async Task<List<ExpenseEntity>> GetAllAsync()
        {
            return await _dbContext.Expenses.AsNoTracking().ToListAsync();
        }

        async Task<ExpenseEntity?> IExpensesReadOnlyRepository.GetByIdAsync(long id)
        {
            return await _dbContext.Expenses
                .AsNoTracking()
                .FirstOrDefaultAsync(exp => exp.Id == id);
        }

        async Task<ExpenseEntity?> IExpenseUpdateOnlyRepository.GetByIdAsync(UserEntity userEntity, long id)
        {
            return await _dbContext
                .Expenses
                .FirstOrDefaultAsync(exp => exp.Id == id && exp.UserId == userEntity.Id);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var result = await _dbContext.Expenses.FirstOrDefaultAsync(exp => exp.Id == id);

            if (result is null)
                return false;

            _dbContext.Expenses.Remove(result);

            return true;
        }

        public void Update(ExpenseEntity entity)
        {
            _dbContext.Expenses.Update(entity);
        }

        public async Task<List<ExpenseEntity>> FilterByMonth(DateOnly date)
        {
            var startDate = new DateTime(
                year: date.Year,
                month: date.Month,
                day: 1).Date;

            var daysInMonth = DateTime.DaysInMonth(
                year: date.Year,
                month: date.Month);

            var endDate = new DateTime(
                year: date.Year,
                month: date.Month,
                day: daysInMonth,
                hour: 23,
                minute: 59,
                second: 59);

            return await _dbContext.Expenses
                .AsNoTracking()
                .Where(exp => exp.Date >= startDate && exp.Date <= endDate)
                .OrderBy(exp => exp.Date)
                .ThenBy(exp => exp.Title)
                .ToListAsync();
        }
    }
}
