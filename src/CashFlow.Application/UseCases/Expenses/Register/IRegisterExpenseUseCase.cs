using CashFlow.Domain.Requests;
using CashFlow.Domain.Responses.Register;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public interface IRegisterExpenseUseCase
    {
        Task<RegisteredExpenseResponse> Execute(ExpenseRequest req);
    }
}
