using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Interface.User
{
    public interface IUserWriteOnlyRepository
    {
        Task Add(UserEntity user);
    }
}
