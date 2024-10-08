﻿using CashFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CashFlow.Domain.Entities
{
    public class ExpenseEntity
    {
        public long Id {  get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public PaymentTypeEnum PaymentType { get; set; }
    }
}
