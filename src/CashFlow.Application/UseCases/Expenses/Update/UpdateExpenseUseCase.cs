using AutoMapper;
using CashFlow.Domain.Responses.Register;
using CashFlow.Domain.Interface;
using CashFlow.Domain.Interface.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CashFlow.Domain.Requests.Expenses;
using CashFlow.Domain.Interface.Service.LoggedUser;

namespace CashFlow.Application.UseCases.Expenses.Update
{
    public class UpdateExpenseUseCase : IUpdateExpenseUseCase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExpenseUpdateOnlyRepository _repository;
        private readonly ILoggedUser _loggedUser;

        public UpdateExpenseUseCase(
            IMapper mapper, 
            IUnitOfWork unitOfWork,
            IExpenseUpdateOnlyRepository expenseUpdateOnlyRepository,
            ILoggedUser loggedUser)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = expenseUpdateOnlyRepository;
            _loggedUser = loggedUser;
        }

        public async Task<RegisteredExpenseResponse> Execute(long id, ExpenseRequest req)
        {
            Validate(req);

            var loggedUser = await _loggedUser.GetAsync();

            var expenseEntity = await _repository.GetByIdAsync(loggedUser, id) ?? 
                throw new NotFoundException(ErrorMessageResource.EXPENSE_NOT_FOUND);

            _mapper.Map(req, expenseEntity);

            _repository.Update(expenseEntity);

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
