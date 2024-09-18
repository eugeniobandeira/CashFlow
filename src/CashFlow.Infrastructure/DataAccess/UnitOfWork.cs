using CashFlow.Domain.Interface;

namespace CashFlow.Infrastructure.DataAccess
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly CashFlowDbContext _dbContext;

        public UnitOfWork(CashFlowDbContext cashFlowDbContext)
        {
            _dbContext = cashFlowDbContext;
        }
        public void Commit() =>  _dbContext.SaveChanges();
    }
}
