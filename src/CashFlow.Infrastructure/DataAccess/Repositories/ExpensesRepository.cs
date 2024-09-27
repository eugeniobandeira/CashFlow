﻿using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories
{
    internal class ExpensesRepository : 
        IExpensesReadOnlyRepository, 
        IExpensesWriteOnlyRepository,
        IExpenseDeleteOnlyRepository,
        IExpenseUpdateOnlyRepository
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

        async Task<ExpenseEntity?> IExpensesReadOnlyRepository.GetByIdAsync(long id)
        {
            return await _context.Expenses.AsNoTracking().FirstOrDefaultAsync(exp => exp.Id == id);
        }

        async Task<ExpenseEntity?> IExpenseUpdateOnlyRepository.GetByIdAsync(long id)
        {
            return await _context.Expenses.FirstOrDefaultAsync(exp => exp.Id == id);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var result = await _context.Expenses.FirstOrDefaultAsync(exp => exp.Id == id);

            if (result is null)
                return false;

            _context.Expenses.Remove(result);

            return true;
        }

        public void Update(ExpenseEntity entity)
        {
            _context.Expenses.Update(entity);
        }
    }
}
