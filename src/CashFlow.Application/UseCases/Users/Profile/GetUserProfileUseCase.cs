using AutoMapper;
using CashFlow.Domain.Interface.Service.LoggedUser;
using CashFlow.Domain.Responses.Users;

namespace CashFlow.Application.UseCases.Users.Profile
{
    public class GetUserProfileUseCase : IGetUserProfileUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IMapper _mapper;

        public GetUserProfileUseCase(ILoggedUser loggedUser, IMapper mapper)
        {
            _loggedUser = loggedUser;
            _mapper = mapper;
        }

        public async Task<UserProfileResponse> Execute()
        {
            var user = await _loggedUser.GetAsync();

            return _mapper.Map<UserProfileResponse>(user);   
        }
    }
}
