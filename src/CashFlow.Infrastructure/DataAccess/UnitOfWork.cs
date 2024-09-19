using CashFlow.Domain.Interface;

namespace CashFlow.Infrastructure.DataAccess
{
    internal class UnitOfWork(CashFlowDbContext cashFlowDbContext) : IUnitOfWork
    {
        private readonly CashFlowDbContext _dbContext = cashFlowDbContext;

        public async Task Commit() => await _dbContext.SaveChangesAsync();
    }
}
