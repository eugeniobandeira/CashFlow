using CashFlow.Domain.Responses.Expenses;

namespace CashFlow.Domain.Responses.Register
{
    public class ExpensesResponseList
    {
        public List<ShortExpenseResponse> Expenses { get; set; } = [];
    }
}
