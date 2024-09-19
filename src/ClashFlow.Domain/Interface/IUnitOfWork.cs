namespace CashFlow.Domain.Interface
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
