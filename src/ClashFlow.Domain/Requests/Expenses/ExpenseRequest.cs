using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Requests.Expenses
{
    public class ExpenseRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public PaymentTypeEnum PaymentType { get; set; }
    }
}
