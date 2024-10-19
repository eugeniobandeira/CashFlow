using AutoMapper;
using CashFlow.Domain.Requests;
using CashFlow.Domain.Responses.Register;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface;
using CashFlow.Domain.Interface.Expenses;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseUseCase : IRegisterExpenseUseCase
    {
        private readonly IExpensesWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper ;

        public RegisterExpenseUseCase(
            IExpensesWriteOnlyRepository expensesRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = expensesRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<RegisteredExpenseResponse> Execute(ExpenseRequest req)
        {
            Validate(req);

            var entity = _mapper.Map<ExpenseEntity>(req);

            await _repository.AddAsync(entity);

            await _unitOfWork.Commit();

            return _mapper.Map<RegisteredExpenseResponse>(entity);
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
