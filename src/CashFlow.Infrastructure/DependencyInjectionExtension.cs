using CashFlow.Domain.Interface;
using CashFlow.Domain.Interface.Expenses;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);  
            AddRepositories(services);  
        }

        #region Private Methods
        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CashflowConnection");

            services.AddDbContext<CashFlowDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
        }
        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IExpensesReadOnlyRepository, ExpensesRepository>();
            services.AddScoped<IExpensesWriteOnlyRepository, ExpensesRepository>();
            services.AddScoped<IExpenseDeleteRepository, ExpensesRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        #endregion
    }
}
