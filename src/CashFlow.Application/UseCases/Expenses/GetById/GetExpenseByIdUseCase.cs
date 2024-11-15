using AutoMapper;
using CashFlow.Domain.Responses.Register;
using CashFlow.Domain.Interface.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CashFlow.Domain.Interface.Service.LoggedUser;

namespace CashFlow.Application.UseCases.Expenses.GetById
{
    public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
    {
        private readonly IExpensesReadOnlyRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public GetExpenseByIdUseCase(
            IExpensesReadOnlyRepository expensesRepository, 
            IMapper mapper,
            ILoggedUser loggedUser)
        {
            _repository = expensesRepository;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }
        public async Task<RegisteredExpenseResponse> Execute(long id)
        {
            var loggedUser = await _loggedUser.GetAsync();

            var result = await _repository.GetByIdAsync(loggedUser, id);

            return result is null
                ? throw new NotFoundException(ErrorMessageResource.EXPENSE_NOT_FOUND)
                : _mapper.Map<RegisteredExpenseResponse>(result);
        }
    }
}
