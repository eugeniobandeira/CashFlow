using CashFlow.Domain.Entities;
using CashFlow.Domain.Helper;
using CashFlow.Domain.Interface.Security.Cryptography;
using CashFlow.Domain.Interface.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test
{
    public class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>
    {
        public ExpenseIdentityManager Regular_User_Expense_Manager { get; private set; } = default!;
        public ExpenseIdentityManager Admin_User_Expense_Manager { get; private set; } = default!;
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
            var regularUser = AddRegularUser(dbContext, passwordEncripter, jwtTokenGenerator);
            var expenseRegularUser = AddExpenses(dbContext, regularUser, expenseId: 1);
            Regular_User_Expense_Manager = new ExpenseIdentityManager(expenseRegularUser);

            var adminUser = AddAdminUser(dbContext, passwordEncripter, jwtTokenGenerator);
            var expenseAdminUser = AddExpenses(dbContext, adminUser, expenseId: 2);
            Admin_User_Expense_Manager = new ExpenseIdentityManager(expenseAdminUser);

            dbContext.SaveChanges();
        }

        #region Helpers
        private UserEntity AddRegularUser(
            CashFlowDbContext dbContext, 
            IPasswordEncripter passwordEncripter, 
            IJwtTokenGenerator jwtTokenGenerator)
        {
            var user = UserBuilder.Build();
            user.Id = 1;

            var password = user.Password;
            user.Password = passwordEncripter.Encrypt(user.Password);

            dbContext.Users.Add(user);

            var token = jwtTokenGenerator.Generate(user);

            Regular_User_Manager = new UserIdentityManager(user, password, token);

            return user;
        }

        private UserEntity AddAdminUser(
            CashFlowDbContext dbContext, 
            IPasswordEncripter passwordEncripter, 
            IJwtTokenGenerator jwtTokenGenerator)
        {
            var user = UserBuilder.Build(RolesHelper.ADMIN);
            user.Id = 2;
            var password = user.Password;

            user.Password = passwordEncripter.Encrypt(user.Password);

            dbContext.Users.Add(user);

            var token = jwtTokenGenerator.Generate(user);

            Admin_User_Manager = new UserIdentityManager(user, password, token);

            return user;
        }

        private static ExpenseEntity AddExpenses(CashFlowDbContext dbContext, UserEntity user, long expenseId)
        {
            var expense = ExpenseBuilder.Build(user);
            expense.Id = expenseId;

            dbContext.Expenses.Add(expense);

            return expense;
        }
        #endregion
    }
}
