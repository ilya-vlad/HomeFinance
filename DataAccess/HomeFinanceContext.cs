using Microsoft.EntityFrameworkCore;
using Common.Models;

namespace DataAccess 
{
    public class HomeFinanceContext : DbContext
    {
        public virtual DbSet<Operation> Operations { get; set; }
        public virtual DbSet<OperationCategory> OperationCategories { get; set; }

        public HomeFinanceContext(DbContextOptions<HomeFinanceContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           
            modelBuilder.Seed();
        }
    }
}