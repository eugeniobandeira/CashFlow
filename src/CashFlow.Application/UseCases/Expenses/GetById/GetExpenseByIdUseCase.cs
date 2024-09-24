using AutoMapper;
using CashFlow.Communication.Responses.Register;
using CashFlow.Domain.Interface.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.GetById
{
    internal class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
    {
        private readonly IExpensesRepository _repository;
        private readonly IMapper _mapper;

        public GetExpenseByIdUseCase(IExpensesRepository expensesRepository, IMapper mapper)
        {
            _repository = expensesRepository;
            _mapper = mapper;
        }
        public async Task<RegisteredExpenseResponse> Execute(long id)
        {
            var result = await _repository.GetExpensebyIdAsync(id);

            if (result is null)
                throw new NotFoundException(ErrorMessageResource.EXPENSE_NOT_FOUND);

            return _mapper.Map<RegisteredExpenseResponse>(result);
        }
    }
}
