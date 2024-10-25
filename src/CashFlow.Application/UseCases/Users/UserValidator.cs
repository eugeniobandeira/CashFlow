using CashFlow.Domain.Requests.Users;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Users
{
    public class UserValidator : AbstractValidator<UserRequest>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage(ErrorMessageResource.EMPTY_NAME);

            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage(ErrorMessageResource.EMPTY_EMAIL)
                .EmailAddress()
                .WithMessage(ErrorMessageResource.INVALID_EMAIL);

            RuleFor(user => user.Password)
                .SetValidator(new PasswordValidator<UserRequest>());
        }
    }
}
