using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess
{
    internal class CashFlowDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<ExpenseEntity> Expenses { get; set; }

    }
}
