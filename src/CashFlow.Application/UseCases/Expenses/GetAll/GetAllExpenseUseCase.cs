
using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Communication.Responses.Register;
using CashFlow.Domain.Interface.Expenses;

namespace CashFlow.Application.UseCases.Expenses.GetAll
{
    internal class GetAllExpenseUseCase(IExpensesReadOnlyRepository expensesRepository, IMapper mapper) : IGetAllExpenseUseCase
    {
        private readonly IExpensesReadOnlyRepository _repository = expensesRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<ExpensesResponseList> Execute()
        {
            var result = await _repository.GetAllAsync();

            return new ExpensesResponseList
            {
                RegisteredExpenses = _mapper.Map<List<ShortExpenseResponse>>(result)
            };
        }
    }
}
