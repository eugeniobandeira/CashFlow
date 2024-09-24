
using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Communication.Responses.Register;
using CashFlow.Domain.Interface.Expenses;

namespace CashFlow.Application.UseCases.Expenses.GetAll
{
    internal class GetAllExpenseUseCase(IExpensesRepository expensesRepository, IMapper mapper) : IGetAllExpenseUseCase
    {
        private readonly IExpensesRepository _repository = expensesRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<ExpensesResponseList> Execute()
        {
            var result = await _repository.GetAll();

            return new ExpensesResponseList
            {
                RegisteredExpenses = _mapper.Map<List<ShortExpenseResponse>>(result)
            };
        }
    }
}
