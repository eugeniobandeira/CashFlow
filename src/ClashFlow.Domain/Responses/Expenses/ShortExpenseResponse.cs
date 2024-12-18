using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Responses.Expenses
{
    public class ShortExpenseResponse
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public IList<TagEnum> Tags { get; set; } = [];
    }
}
