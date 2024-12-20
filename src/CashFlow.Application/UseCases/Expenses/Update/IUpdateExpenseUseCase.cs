﻿using CashFlow.Domain.Requests.Expenses;
using CashFlow.Domain.Responses.Register;

namespace CashFlow.Application.UseCases.Expenses.Update
{
    public interface IUpdateExpenseUseCase
    {
        Task<RegisteredExpenseResponse> Execute(long id, ExpenseRequest req);
    }
}
