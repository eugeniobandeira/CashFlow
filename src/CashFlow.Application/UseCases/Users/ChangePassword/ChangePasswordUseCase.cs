using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface;
using CashFlow.Domain.Interface.Security.Cryptography;
using CashFlow.Domain.Interface.Service.LoggedUser;
using CashFlow.Domain.Interface.User;
using CashFlow.Domain.Requests.Users;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.ChangePassword
{
    public class ChangePasswordUseCase : IChangePasswordUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserUpdateOnlyRepository _updateRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordEncripter _passwordEncripter;

        public ChangePasswordUseCase(
            ILoggedUser loggedUser,
            IPasswordEncripter passwordEncripter,
            IUserUpdateOnlyRepository userUpdateOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _updateRepository = userUpdateOnlyRepository;
            _unitOfWork = unitOfWork;
            _passwordEncripter = passwordEncripter;
        }

        public async Task Execute(ChangePasswordRequest req)
        {
            var loggedUser = await _loggedUser.GetAsync();

            Validate(req, loggedUser);

            var user = await _updateRepository.GetByIdAsync(loggedUser.Id);
            user.Password = _passwordEncripter.Encrypt(req.NewPassword);

            _updateRepository.Update(user);

            await _unitOfWork.Commit();
        }

        private void Validate(ChangePasswordRequest req, UserEntity loggedUser)
        {
            var validator = new ChangePasswordValidator();

            var result = validator.Validate(req);

            var passwordMatch = _passwordEncripter.Verify(req.Password, loggedUser.Password);

            if (!passwordMatch)
                result.Errors.Add(new ValidationFailure(string.Empty, ErrorMessageResource.PASSWORD_DIFFERNT_CURRENT_PASSWORD));

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
