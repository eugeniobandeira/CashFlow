using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses.Register;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public interface IRegisterExpenseUseCase
    {
        RegisteredExpenseResponse Execute(InsertExpenseRequest req);
    }
}
