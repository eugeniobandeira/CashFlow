namespace CashFlow.Domain.Interface.Expenses
{
    public interface IExpenseDeleteOnlyRepository
    {
        /// <summary>
        /// This function returns TRUE if the deletion was successfull
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(long id);
    }
}
