using CashFlow.Domain.Requests.Users;

namespace CashFlow.Application.UseCases.Users.ChangePassword
{
    public interface IChangePasswordUseCase
    {
        Task Execute(ChangePasswordRequest req);
    }
}
