namespace CashFlow.Domain.Responses
{
    public class ShortExpenseResponse
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
