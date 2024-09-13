using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using FluentValidation;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseValidator : AbstractValidator<InsertExpenseRequest>
    {
        public RegisterExpenseValidator()
        {
            RuleFor(expense => expense.Title).NotEmpty().WithMessage("The title can not be null or whitespace");
            RuleFor(expense => expense.Amount).GreaterThan(0).WithMessage("The amount must be higher than zero");
            RuleFor(expense => expense.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("It is not allowed to use future date");
            RuleFor(expense => expense.PaymentType).IsInEnum().WithMessage("Payment type not valid");
        }
    }
}
