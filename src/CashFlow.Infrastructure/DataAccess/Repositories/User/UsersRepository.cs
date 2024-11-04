using CashFlow.Domain.Entities;
using CashFlow.Domain.Interface.User;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories.User
{
    internal class UsersRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
    {
        private readonly CashFlowDbContext _context;

        public UsersRepository(CashFlowDbContext cashFlowDbContext)
        {
            _context = cashFlowDbContext;
        }

        public async Task Add(UserEntity user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<bool> ExistUserWithEmail(string email)
        {
            return await _context.Users.AnyAsync(user => user.Email.Equals(email));    
        }

        public async Task<UserEntity?> GetUserByEmail(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
        }
    }
}
