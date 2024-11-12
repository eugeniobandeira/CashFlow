using AutoMapper;
using CashFlow.Domain.Responses.Register;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface;
using CashFlow.Domain.Interface.Expenses;
using CashFlow.Exception.ExceptionBase;
using CashFlow.Domain.Requests.Expenses;
using CashFlow.Domain.Interface.Service.LoggedUser;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseUseCase : IRegisterExpenseUseCase
    {
        private readonly IExpensesWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public RegisterExpenseUseCase(
            IExpensesWriteOnlyRepository expensesRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggedUser loggedUser)
        {
            _repository = expensesRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<RegisteredExpenseResponse> Execute(ExpenseRequest req)
        {
            Validate(req);

            var loggedUser = await _loggedUser.GetAsync();

            var expenseEntity = _mapper.Map<ExpenseEntity>(req);
            expenseEntity.UserId = loggedUser.Id;

            await _repository.AddAsync(expenseEntity);

            await _unitOfWork.Commit();

            return _mapper.Map<RegisteredExpenseResponse>(expenseEntity);
        }

        private static void Validate(ExpenseRequest req)
        {
            var validator = new ExpenseValidator();

            var result = validator.Validate(req);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
