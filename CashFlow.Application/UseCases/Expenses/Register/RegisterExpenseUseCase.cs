using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseUseCase
    {
        public RegisteredExpenseResponse Execute(InsertExpenseRequest req)
        {
            return new RegisteredExpenseResponse();
        }
    }
}
