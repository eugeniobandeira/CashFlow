using CashFlow.Domain.Requests.Users;
using CashFlow.Domain.Responses.Users;

namespace CashFlow.Application.UseCases.Users.Update
{
    public interface IUpdateUserUseCase
    {
        Task<UserProfileResponse> Execute(UpdateUserRequest req);
    }
}
