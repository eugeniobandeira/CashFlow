using CashFlow.Domain.Requests.Login;
using CashFlow.Domain.Responses.Users;

namespace CashFlow.Application.UseCases.Login
{
    public interface IDoLoginUseCase
    {
        Task<RegisteredUserResponse> Execute(LoginRequest req);
    }
}
