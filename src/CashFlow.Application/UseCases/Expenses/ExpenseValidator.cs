using CashFlow.Domain.Requests;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Expenses
{
    public class ExpenseValidator : AbstractValidator<ExpenseRequest>
    {
        public ExpenseValidator()
        {
            RuleFor(expense => expense.Title).NotEmpty().WithMessage(ErrorMessageResource.TITLE_REQUIRED);
            RuleFor(expense => expense.Amount).GreaterThan(0).WithMessage(ErrorMessageResource.AMOUNT_MUST_BE_GREATER_THAN_ZERO);
            RuleFor(expense => expense.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ErrorMessageResource.EXPENSE_DATA_INVALID);
            RuleFor(expense => expense.PaymentType).IsInEnum().WithMessage(ErrorMessageResource.PAYMENT_TYPE_INVALID);
        }
    }
}
