using AutoMapper;
using CashFlow.Domain.Responses.Register;
using CashFlow.Domain.Interface.Expenses;
using CashFlow.Domain.Responses.Expenses;
using CashFlow.Domain.Interface.Service.LoggedUser;

namespace CashFlow.Application.UseCases.Expenses.GetAll
{
    public class GetAllExpenseUseCase : IGetAllExpenseUseCase
    {
        private readonly IExpensesReadOnlyRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public GetAllExpenseUseCase(
            IExpensesReadOnlyRepository expensesRepository, 
            IMapper mapper, 
            ILoggedUser loggedUser)
        {
            _repository = expensesRepository;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<ExpensesResponseList> Execute()
        {
            var loggedUser = await _loggedUser.GetAsync();

            var result = await _repository.GetAllAsync(loggedUser);

            return new ExpensesResponseList
            {
                RegisteredExpenses = _mapper.Map<List<ShortExpenseResponse>>(result)
            };
        }
    }
}
