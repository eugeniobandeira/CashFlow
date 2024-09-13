using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseUseCase
    {
        public static RegisteredExpenseResponse Execute(InsertExpenseRequest req)
        {
            Validate(req);

            return new RegisteredExpenseResponse();
        }

        private static void Validate(InsertExpenseRequest req) 
        { 
            var isTitleEmpty = string.IsNullOrWhiteSpace(req.Title);
            if (isTitleEmpty)
                throw new ArgumentException("The title can not be null or whitespace");

            if (req.Amount <= 0)
                throw new ArgumentException("The amount must be higher than 0");

            var result = DateTime.Compare(req.Date, DateTime.UtcNow);
            if (result > 0)
                throw new ArgumentException("It is not allowed to use future date");

            var isPaymentTypeValid = Enum.IsDefined(typeof(PaymentType), req.Type);
            if (!isPaymentTypeValid)
                throw new ArgumentException("Payment type not valid");
        }
    }
}
