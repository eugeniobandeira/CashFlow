using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface.Expenses;

namespace CashFlow.Infrastructure.DataAccess.Repositories
{
    internal class ExpensesRepository : IExpensesRepository
    {
        private readonly CashFlowDbContext _context;

        public ExpensesRepository(CashFlowDbContext cashFlowDbContext)
        {
            _context = cashFlowDbContext;
        }
        public void Add(ExpenseEntity expenseEntity)
        {
            _context.Expenses.Add(expenseEntity);
        }
    }
}
