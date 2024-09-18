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

        public RegisterExpenseUseCase(IExpensesRepository expensesRepository, IUnitOfWork unitOfWork)
        {
            _repository = expensesRepository;
            _unitOfWork = unitOfWork;
        }

        public RegisteredExpenseResponse Execute(InsertExpenseRequest req)
        {
            Validate(req);

            var entity = new ExpenseEntity
            {
                ID_EXPENSE = 100,
                VL_AMOUNT = req.Amount,
                CD_TITLE = req.Title,
                DS_DESCRIPTION = req.Description,
                DT_DATE = req.Date,
                TP_PAYMENT = (Domain.Enums.PaymentTypeEnum)req.PaymentType,
            };

            _repository.Add(entity);
            _unitOfWork.Commit();

            return new RegisteredExpenseResponse();
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
