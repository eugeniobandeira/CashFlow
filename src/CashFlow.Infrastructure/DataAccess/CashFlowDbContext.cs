using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess
{
    public class CashFlowDbContext : DbContext
    {
        public CashFlowDbContext(DbContextOptions<CashFlowDbContext> options) : base(options) { }
        public DbSet<ExpenseEntity> Expenses { get; set; }
        public DbSet<UserEntity> Users { get; set; }

    }
}
