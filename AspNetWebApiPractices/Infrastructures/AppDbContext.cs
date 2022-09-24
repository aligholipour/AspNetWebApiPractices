using AspNetWebApiPractices.Domain.Customer;
using Microsoft.EntityFrameworkCore;

namespace AspNetWebApiPractices.Infrastructures
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
            DbInitializer.Seed(modelBuilder);
        }
    }
}
