using CashFlow.Domain.Responses.Register;

namespace CashFlow.Application.UseCases.Expenses.GetById
{
    public interface IGetExpenseByIdUseCase
    {
        Task<RegisteredExpenseResponse> Execute(long id);
    }
}
