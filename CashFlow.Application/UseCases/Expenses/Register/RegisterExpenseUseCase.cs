using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses.Register;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseUseCase
    {
        public RegisteredExpenseResponse Execute(InsertExpenseRequest req)
        {
            Validate(req);

            return new RegisteredExpenseResponse();
        }

        private static void Validate(InsertExpenseRequest req)
        {
            var validator = new RegisterExpenseValidator();

            var result = validator.Validate(req);

            if (!result.IsValid)
            {
                result.Errors.Select(error => error.ErrorMessage).ToList();

                throw;
            }
        }
    }
}
