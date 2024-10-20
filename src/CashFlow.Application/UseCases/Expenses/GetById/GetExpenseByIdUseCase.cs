﻿using AutoMapper;
using CashFlow.Domain.Responses.Register;
using CashFlow.Domain.Interface.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.GetById
{
    internal class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
    {
        private readonly IExpensesReadOnlyRepository _repository;
        private readonly IMapper _mapper;

        public GetExpenseByIdUseCase(IExpensesReadOnlyRepository expensesRepository, IMapper mapper)
        {
            _repository = expensesRepository;
            _mapper = mapper;
        }
        public async Task<RegisteredExpenseResponse> Execute(long id)
        {
            var result = await _repository.GetByIdAsync(id);

            return result is null
                ? throw new NotFoundException(ErrorMessageResource.EXPENSE_NOT_FOUND)
                : _mapper.Map<RegisteredExpenseResponse>(result);
        }
    }
}
