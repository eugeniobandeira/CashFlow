using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses.Register;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface;
using CashFlow.Domain.Interface.Expenses;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseUseCase : IRegisterExpenseUseCase
    {
        private readonly IExpensesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper ;

        public RegisterExpenseUseCase(
            IExpensesRepository expensesRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = expensesRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<RegisteredExpenseResponse> Execute(InsertExpenseRequest req)
        {
            Validate(req);

            var entity = _mapper.Map<ExpenseEntity>(req);

            await _repository.Add(entity);

            await _unitOfWork.Commit();

            return _mapper.Map<RegisteredExpenseResponse>(entity);
        }

        private static void Validate(InsertExpenseRequest req)
        {
            var validator = new RegisterExpenseValidator();

            var result = validator.Validate(req);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
