using CashFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CashFlow.Domain.Entities
{
    public class ExpenseEntity
    {
        [Key]
        public long ID_EXPENSE {  get; set; }
        public string CD_TITLE { get; set; } = string.Empty;
        public string? DS_DESCRIPTION { get; set; }
        public DateTime DT_DATE { get; set; }
        public decimal VL_AMOUNT { get; set; }
        public PaymentTypeEnum TP_PAYMENT { get; set; }
    }
}
