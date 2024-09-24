using CashFlow.Communication.Enums;

namespace CashFlow.Communication.Responses.Register
{
    public class RegisteredExpenseResponse
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public PaymentTypeEnum PaymentType { get; set; }
        public DateTime Date { get; set; }
    }
}
