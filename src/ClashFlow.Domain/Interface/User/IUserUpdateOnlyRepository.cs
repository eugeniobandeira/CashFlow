using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Interface.User
{
    public interface IUserUpdateOnlyRepository
    {
        Task<UserEntity> GetByIdAsync(long id);
        void Update(UserEntity req);
    }
}
