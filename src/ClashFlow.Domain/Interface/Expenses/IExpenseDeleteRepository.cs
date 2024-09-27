namespace CashFlow.Domain.Interface.Expenses
{
    public interface IExpenseDeleteRepository
    {
        /// <summary>
        /// This function returns TRUE if the deletion was successfull
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(long id);
    }
}
