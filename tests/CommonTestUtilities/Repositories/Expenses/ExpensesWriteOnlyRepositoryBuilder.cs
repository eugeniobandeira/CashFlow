using CashFlow.Domain.Interface.Expenses;
using Moq;

namespace CommonTestUtilities.Repositories.Expenses
{
    public class ExpensesWriteOnlyRepositoryBuilder
    {
        public static IExpensesWriteOnlyRepository Build()
        {
            var mock = new Mock<IExpensesWriteOnlyRepository>();

            return mock.Object;
        }
    }
}
