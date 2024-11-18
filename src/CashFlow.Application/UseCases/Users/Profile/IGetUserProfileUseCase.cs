using CashFlow.Domain.Responses.Users;

namespace CashFlow.Application.UseCases.Users.Profile
{
    public interface IGetUserProfileUseCase
    {
        Task<UserProfileResponse> Execute();
    }
}
