using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface.Security.Cryptography;
using CashFlow.Domain.Interface.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using WebApi.Test.Resources;

namespace WebApi.Test
{
    public class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>
    {
        public ExpenseIdentityManager Expense_Manager { get; private set; } = default!;
        public UserIdentityManager Regular_User_Manager { get; private set; } = default!;
        public UserIdentityManager Admin_User_Manager { get; private set; } = default!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    services.AddDbContext<CashFlowDbContext>(config =>
                    {
                        config.UseInMemoryDatabase("InMemoryDbForTesting");
                        config.UseInternalServiceProvider(provider);
                    });

                    var scope = services.BuildServiceProvider().CreateAsyncScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                    var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
                    var jwtTokenGenerator = scope.ServiceProvider.GetRequiredService<IJwtTokenGenerator>();

                    StartDatabase(dbContext, passwordEncripter, jwtTokenGenerator);

                });

        }

        private void StartDatabase(
            CashFlowDbContext dbContext, 
            IPasswordEncripter passwordEncripter,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            var user = AddRegularUser(dbContext, passwordEncripter, jwtTokenGenerator);
            AddExpenses(dbContext, user);

            dbContext.SaveChanges();
        }

        #region Helpers

        private UserEntity AddRegularUser(
            CashFlowDbContext dbContext, 
            IPasswordEncripter passwordEncripter, 
            IJwtTokenGenerator jwtTokenGenerator)
        {
            var user = UserBuilder.Build();
            var password = user.Password;

            user.Password = passwordEncripter.Encrypt(user.Password);

            dbContext.Users.Add(user);

            var token = jwtTokenGenerator.Generate(user);

            Regular_User_Manager = new UserIdentityManager(user, password, token);

            return user;
        }

        private void AddExpenses(CashFlowDbContext dbContext, UserEntity user)
        {
            var expense = ExpenseBuilder.Build(user);

            dbContext.Expenses.Add(expense);

            Expense_Manager = new ExpenseIdentityManager(expense);
        }
        #endregion
    }
}
