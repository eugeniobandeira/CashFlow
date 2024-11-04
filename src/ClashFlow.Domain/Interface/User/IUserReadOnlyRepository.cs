using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Interface.User
{
    public interface IUserReadOnlyRepository
    {
        Task<bool> ExistUserWithEmail(string email);
        Task<UserEntity?> GetUserByEmail(string email);
    }
}
