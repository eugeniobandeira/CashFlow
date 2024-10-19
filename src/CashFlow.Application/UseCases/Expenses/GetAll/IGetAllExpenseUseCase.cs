using CashFlow.Domain.Responses.Register;

namespace CashFlow.Application.UseCases.Expenses.GetAll
{
    public interface IGetAllExpenseUseCase
    {
        Task<ExpensesResponseList> Execute();
    }
}
