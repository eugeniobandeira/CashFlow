using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories
{
    internal class ExpensesRepository : IExpensesRepository
    {
        private readonly CashFlowDbContext _context;

        public ExpensesRepository(CashFlowDbContext cashFlowDbContext)
        {
            _context = cashFlowDbContext;
        }

        public async Task Add(ExpenseEntity expenseEntity)
        {
            await _context.Expenses.AddAsync(expenseEntity);
        }

        public async Task<List<ExpenseEntity>> GetAll()
        {
            return await _context.Expenses.ToListAsync();
        }
    }
}
