using CashFlow.Domain.Interface;
using CashFlow.Domain.Interface.Service.LoggedUser;
using CashFlow.Domain.Interface.User;
using CashFlow.Domain.Requests.Users;
using CashFlow.Domain.Responses.Users;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Users.Update
{
    public class UpdateUserUseCase : IUpdateUserUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserUpdateOnlyRepository _repository;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserUseCase(
            ILoggedUser loggedUser,
            IUserUpdateOnlyRepository userUpdateOnlyRepository,
            IUserReadOnlyRepository userReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _userReadOnlyRepository = userReadOnlyRepository;
            _repository = userUpdateOnlyRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<UserProfileResponse> Execute(UpdateUserRequest req)
        {
            var loggedUser = await _loggedUser.GetAsync();

            await Validate(req, loggedUser.Email);

            var user = await _repository.GetByIdAsync(loggedUser.Id);

            user.Name = req.Name;
            user.Email = req.Email;

            _repository.Update(user);

            await _unitOfWork.Commit();

            return new UserProfileResponse
            {
                Name = user.Name,
                Email = user.Email
            };
        }

        private async Task Validate(UpdateUserRequest req, string currentEmail)
        {
            var validator = new UpdateUserValidator();

            var result = validator.Validate(req);

            if (!currentEmail.Equals(req.Email))
            {
                var userExist = await _userReadOnlyRepository.ExistUserWithEmail(req.Email);
                if (userExist)
                    result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ErrorMessageResource.EMAIL_ALREADY_REGISTERED));
            }

            if (!result.IsValid)
            {
                var errorMessage = result.Errors.Select(err => err.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessage);
            }
        }
    }
}
