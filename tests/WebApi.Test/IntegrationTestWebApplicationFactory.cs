using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface.Security.Cryptography;
using CashFlow.Domain.Interface.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test
{
    public class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>
    {
        private UserEntity? _user;
        private string? _password;
        private string? _token;

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

                    StartDatabase(dbContext, passwordEncripter);

                    var tokenGenerator = scope.ServiceProvider.GetRequiredService<IJwtTokenGenerator>();
                    _token = tokenGenerator.Generate(_user!);
                });

        }

        private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter)
        {
            _user = UserBuilder.Build();
            _password = _user.Password;
            _user.Password = passwordEncripter.Encrypt(_user.Password);

            dbContext.Users.Add(_user);
            dbContext.SaveChanges();
        }

        #region Get Methods
        public string GetEmail()
            => _user!.Email;

        public string GetName()
            => _user!.Name;

        public string GetPassword()
            => _password!;

        public string GetToken()
            => _token!;
        #endregion
    }
}
