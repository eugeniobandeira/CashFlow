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

        public async Task<List<ExpenseEntity>> GetAllAsync(UserEntity userEntity)
        {
            return await _dbContext
                .Expenses
                .AsNoTracking()
                .Where(exp => exp.UserId == userEntity.Id)
                .ToListAsync();
        }

        async Task<ExpenseEntity?> IExpensesReadOnlyRepository.GetByIdAsync(UserEntity userEntity, long id)
        {
            return await _dbContext.Expenses
                .AsNoTracking()
                .FirstOrDefaultAsync(exp => exp.Id == id && exp.UserId == userEntity.Id);
        }

        async Task<ExpenseEntity?> IExpenseUpdateOnlyRepository.GetByIdAsync(UserEntity userEntity, long id)
        {
            return await _dbContext
                .Expenses
                .FirstOrDefaultAsync(exp => exp.Id == id && exp.UserId == userEntity.Id);
        }

        public async Task DeleteAsync(long id)
        {
            var result = await _dbContext.Expenses.FindAsync(id);

            _dbContext
                .Expenses
                .Remove(result!);
        }

        public void Update(ExpenseEntity entity)
        {
            _dbContext
                .Expenses
                .Update(entity);
        }

        public async Task<List<ExpenseEntity>> FilterByMonth(UserEntity userEntity, DateOnly date)
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
                .Where(exp => exp.UserId == userEntity.Id && exp.Date >= startDate && exp.Date <= endDate)
                .OrderBy(exp => exp.Date)
                .ThenBy(exp => exp.Title)
                .ToListAsync();
        }
    }
}
