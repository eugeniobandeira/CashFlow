using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Entities
{
    public class TagEntity
    {
        public long Id { get; set; }
        public TagEnum Value { get; set; }

        public long ExpenseId { get; set; }
        public ExpenseEntity Expense { get; set; } = default!;
    }
}
