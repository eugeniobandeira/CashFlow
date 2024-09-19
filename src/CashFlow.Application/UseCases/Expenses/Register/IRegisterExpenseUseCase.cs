using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses.Register;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public interface IRegisterExpenseUseCase
    {
        Task<RegisteredExpenseResponse> Execute(InsertExpenseRequest req);
    }
}
