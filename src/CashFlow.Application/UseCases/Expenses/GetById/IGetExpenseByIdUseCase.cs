using CashFlow.Communication.Responses.Register;

namespace CashFlow.Application.UseCases.Expenses.GetById
{
    public interface IGetExpenseByIdUseCase
    {
        Task<RegisteredExpenseResponse> Execute(long id);
    }
}
