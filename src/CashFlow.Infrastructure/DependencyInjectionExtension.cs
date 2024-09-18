using CashFlow.Domain.Interface.Expenses;
using CashFlow.Infrastructure.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IExpensesRepository, ExpensesRepository>();
        }
    }
}
