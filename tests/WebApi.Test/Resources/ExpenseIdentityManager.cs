﻿using CashFlow.Domain.Entities;

namespace WebApi.Test.Resources
{
    public class ExpenseIdentityManager
    {
        private readonly ExpenseEntity _expense;

        public ExpenseIdentityManager(ExpenseEntity expense)
        {
            _expense = expense;
        }

        public long GetExpenseId()
            => _expense!.Id;
    }
}
