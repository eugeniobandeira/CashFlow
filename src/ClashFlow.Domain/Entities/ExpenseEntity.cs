using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Entities
{
    public class ExpenseEntity
    {
        public long ID_EXPENSE {  get; set; }
        public string CD_TITLE { get; set; } = string.Empty;
        public string? DS_DESCRIPTION { get; set; }
        public DateTime DT_DATE { get; set; }
        public decimal VL_AMOUNT { get; set; }
        public PaymentTypeEnum TP_PAYMENT_TYPE { get; set; }
    }
}
