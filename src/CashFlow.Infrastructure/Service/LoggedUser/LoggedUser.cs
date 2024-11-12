using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface.Security.Tokens;
using CashFlow.Domain.Interface.Service.LoggedUser;
using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CashFlow.Infrastructure.Service.LoggedUser
{
    internal class LoggedUser : ILoggedUser
    {
        private readonly CashFlowDbContext _dbContext;
        private readonly ITokenProvider _tokenProvider;

        public LoggedUser(CashFlowDbContext cashFlowDbContext, ITokenProvider tokenProvider)
        {
            _dbContext = cashFlowDbContext;
            _tokenProvider = tokenProvider;
        }

        public async Task<UserEntity> GetAsync()
        {
            string token = _tokenProvider.TokenOnRequest();

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
            var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

            return await _dbContext
                .Users
                .AsNoTracking()
                .FirstAsync(user => user.UserId == Guid.Parse(identifier));
        }
    }
}
