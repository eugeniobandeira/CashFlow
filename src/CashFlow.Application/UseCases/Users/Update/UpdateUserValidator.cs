using CashFlow.Domain.Requests.Users;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Users.Update
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage(ErrorMessageResource.EMPTY_NAME);
            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage(ErrorMessageResource.EMPTY_EMAIL)
                .EmailAddress()
                .When(user => string.IsNullOrWhiteSpace(user.Email) == false, ApplyConditionTo.CurrentValidator)
                .WithMessage(ErrorMessageResource.INVALID_EMAIL);
        }
    }
}
