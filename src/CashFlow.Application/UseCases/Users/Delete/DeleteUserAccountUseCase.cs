using CashFlow.Domain.Interface;
using CashFlow.Domain.Interface.Service.LoggedUser;
using CashFlow.Domain.Interface.User;

namespace CashFlow.Application.UseCases.Users.Delete
{
    public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggedUser _loggedUser;
        private readonly IUserDeleteOnlyRepository _userDeleteOnlyRepository;

        public DeleteUserAccountUseCase(
            IUnitOfWork unitOfWork,
            ILoggedUser loggedUser,
            IUserDeleteOnlyRepository userDeleteOnlyRepository)
        {
            _unitOfWork = unitOfWork;
            _loggedUser = loggedUser;
            _userDeleteOnlyRepository = userDeleteOnlyRepository;
        }

        public async Task Execute()
        {
            var user = await _loggedUser.GetAsync();
            await _userDeleteOnlyRepository.DeleteAsync(user);

            await _unitOfWork.Commit();
        }
    }
}
