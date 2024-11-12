using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Interface.Service.LoggedUser
{
    public interface ILoggedUser
    {
        Task<UserEntity> GetAsync();
    }
}
