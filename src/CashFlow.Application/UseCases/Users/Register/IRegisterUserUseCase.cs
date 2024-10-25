using CashFlow.Domain.Requests.Users;
using CashFlow.Domain.Responses.Users;

namespace CashFlow.Application.UseCases.Users.Register
{
    public interface IRegisterUserUseCase
    {
        Task<RegisteredUserResponse> Execute(UserRequest req);
    }
}
