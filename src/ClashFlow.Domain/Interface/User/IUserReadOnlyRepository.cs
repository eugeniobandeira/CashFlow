namespace CashFlow.Domain.Interface.User
{
    public interface IUserReadOnlyRepository
    {
        Task<bool> ExistUserWithEmail(string email);
    }
}
