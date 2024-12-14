using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Interface.User
{
    public interface IUserDeleteOnlyRepository
    {
        Task DeleteAsync(UserEntity user);
    }
}
