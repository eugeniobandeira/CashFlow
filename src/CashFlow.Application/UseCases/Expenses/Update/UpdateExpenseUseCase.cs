using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses.Register;
using CashFlow.Domain.Interface;
using CashFlow.Domain.Interface.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.Update
{
    internal class UpdateExpenseUseCase : IUpdateExpenseUseCase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExpenseUpdateOnlyRepository _repository;

        public UpdateExpenseUseCase(
            IMapper mapper, 
            IUnitOfWork unitOfWork,
            IExpenseUpdateOnlyRepository expenseUpdateOnlyRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = expenseUpdateOnlyRepository;
        }

        public async Task<RegisteredExpenseResponse> Execute(long id, ExpenseRequest req)
        {
            Validate(req);

            var entity = await _repository.GetByIdAsync(id) ?? 
                throw new NotFoundException(ErrorMessageResource.EXPENSE_NOT_FOUND);

            _mapper.Map(req, entity);

            _repository.Update(entity);

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
