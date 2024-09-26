using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories
{
    internal class ExpensesRepository : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository
    {
        private readonly CashFlowDbContext _context;

        public ExpensesRepository(CashFlowDbContext cashFlowDbContext)
        {
            _context = cashFlowDbContext;
        }

        public async Task AddAsync(ExpenseEntity expenseEntity)
        {
            await _context.Expenses.AddAsync(expenseEntity);
        }

        public async Task<List<ExpenseEntity>> GetAllAsync()
        {
            return await _context.Expenses.AsNoTracking().ToListAsync();
        }

        public async Task<ExpenseEntity?> GetByIdAsync(long id)
        {
            return await _context.Expenses.AsNoTracking().FirstOrDefaultAsync(exp => exp.Id == id);
        }
    }
}
