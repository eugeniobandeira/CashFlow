using CashFlow.Domain.Interface;
using CashFlow.Domain.Interface.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.Delete
{
    internal class DeleteExpenseUseCase : IDeleteExpenseUseCase
    {
        private readonly IExpenseDeleteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteExpenseUseCase(
            IExpenseDeleteOnlyRepository expenseDeleteRepository,
            IUnitOfWork unitOfWork)
        {
            _repository = expenseDeleteRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Execute(long id)
        {
            var result = await _repository.DeleteAsync(id);

            if (result is false)
            {
                throw new NotFoundException(ErrorMessageResource.EXPENSE_NOT_FOUND);
            }

            await _unitOfWork.Commit();
        }
    }
}
