using AspNetWebApiPractices.Domain.Customer;
using Microsoft.EntityFrameworkCore;

namespace AspNetWebApiPractices.Infrastructures
{
    public static class DbInitializer
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    FullName = "Roy Fielding"
                },
                new Customer
                {
                    Id = 2,
                    FullName = "John Doe"
                },
                new Customer
                {
                    Id = 3,
                    FullName = "Ali Gholipour"
                }
            );
        }
    }
}
