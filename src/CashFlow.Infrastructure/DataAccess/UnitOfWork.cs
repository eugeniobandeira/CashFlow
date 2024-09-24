using CashFlow.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly CashFlowDbContext _dbContext;

        public UnitOfWork(CashFlowDbContext cashFlowDbContext)
        {
            _dbContext = cashFlowDbContext;
        }
        public async Task Commit()
        {
            try
            {
                if (_dbContext.ChangeTracker.HasChanges())
                    await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex) 
            { 
                throw new Exception($"Error: ", ex);
            }
        }
    }
}
