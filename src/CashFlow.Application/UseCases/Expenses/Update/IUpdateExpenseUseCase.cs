using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses.Register;

namespace CashFlow.Application.UseCases.Expenses.Update
{
    public interface IUpdateExpenseUseCase
    {
        Task<RegisteredExpenseResponse> Execute(long id, ExpenseRequest req);
    }
}
