using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface.User;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories.User
{
    internal class UsersRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
    {
        private readonly CashFlowDbContext _dbContext;

        public UsersRepository(CashFlowDbContext cashFlowDbContext)
        {
            _dbContext = cashFlowDbContext;
        }

        public async Task Add(UserEntity user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public async Task<bool> ExistUserWithEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));    
        }

        public async Task<UserEntity?> GetUserByEmail(string email)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
        }

        public async Task<UserEntity> GetByIdAsync(long id)
        {
            return await _dbContext.Users.FirstAsync(user => user.Id == id);
        }

        public void Update(UserEntity req)
        {
            _dbContext.Users.Update(req);
        }
    }
}
