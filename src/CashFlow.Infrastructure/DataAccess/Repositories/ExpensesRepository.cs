using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface.Expenses;

namespace CashFlow.Infrastructure.DataAccess.Repositories
{
    internal class ExpensesRepository(CashFlowDbContext cashFlowDbContext) : IExpensesRepository
    {
        private readonly CashFlowDbContext _context = cashFlowDbContext;

        public async Task Add(ExpenseEntity expenseEntity)
        {
            await _context.Expenses.AddAsync(expenseEntity);
        }
    }
}
