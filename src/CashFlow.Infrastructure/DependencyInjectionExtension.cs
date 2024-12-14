using CashFlow.Domain.Interface;
using CashFlow.Domain.Interface.Expenses;
using CashFlow.Domain.Interface.Security.Cryptography;
using CashFlow.Domain.Interface.Security.Tokens;
using CashFlow.Domain.Interface.Service.LoggedUser;
using CashFlow.Domain.Interface.User;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.DataAccess.Repositories.Expense;
using CashFlow.Infrastructure.DataAccess.Repositories.User;
using CashFlow.Infrastructure.Extensions;
using CashFlow.Infrastructure.Security.Cryptography;
using CashFlow.Infrastructure.Security.Tokens;
using CashFlow.Infrastructure.Service.LoggedUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPasswordEncripter, PasswordEncripter>();
            services.AddScoped<ILoggedUser, LoggedUser>();

            AddToken(services, configuration);
            AddRepositories(services);

            if (configuration.IsTestEnvironment() is false)
                AddDbContext(services, configuration);
        }

        #region Private Services
        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CashflowConnection");

            services.AddDbContext<CashFlowDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IExpensesReadOnlyRepository, ExpensesRepository>();
            services.AddScoped<IExpensesWriteOnlyRepository, ExpensesRepository>();
            services.AddScoped<IExpenseDeleteOnlyRepository, ExpensesRepository>();
            services.AddScoped<IExpenseUpdateOnlyRepository, ExpensesRepository>();
            services.AddScoped<IUserReadOnlyRepository, UsersRepository>();
            services.AddScoped<IUserWriteOnlyRepository, UsersRepository>();
            services.AddScoped<IUserUpdateOnlyRepository, UsersRepository>();
            services.AddScoped<IUserDeleteOnlyRepository, UsersRepository>();
        }

        private static void AddToken(IServiceCollection services, IConfiguration configuration)
        {
            var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");

            var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

            services.AddScoped<IJwtTokenGenerator>(config => new JwtTokenGenetator(expirationTimeMinutes, signingKey!));
        }
        #endregion
    }
}
