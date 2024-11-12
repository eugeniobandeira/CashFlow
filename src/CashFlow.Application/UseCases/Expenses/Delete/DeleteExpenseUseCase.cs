using CashFlow.Domain.Interface;
using CashFlow.Domain.Interface.Expenses;
using CashFlow.Domain.Interface.Service.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.Delete
{
    internal class DeleteExpenseUseCase : IDeleteExpenseUseCase
    {
        private readonly IExpenseDeleteOnlyRepository _repository;
        private readonly IExpensesReadOnlyRepository _readOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggedUser _loggedUser;

        public DeleteExpenseUseCase(
            IExpenseDeleteOnlyRepository expenseDeleteRepository,
            IExpensesReadOnlyRepository readOnlyRepository,
            IUnitOfWork unitOfWork,
            ILoggedUser loggedUser)
        {
            _repository = expenseDeleteRepository;
            _unitOfWork = unitOfWork;
            _loggedUser = loggedUser;
            _readOnlyRepository = readOnlyRepository;
        }
        public async Task Execute(long id)
        {
            var loggedUser = await _loggedUser.GetAsync();

            var expense = await _readOnlyRepository.GetByIdAsync(loggedUser, id) ?? 
                throw new NotFoundException(ErrorMessageResource.EXPENSE_NOT_FOUND);

            await _repository.DeleteAsync(id);

            await _unitOfWork.Commit();
        }
    }
}
