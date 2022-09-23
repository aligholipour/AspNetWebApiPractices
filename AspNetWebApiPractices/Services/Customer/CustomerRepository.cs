using AspNetWebApiPractices.Infrastructures;
using AspNetWebApiPractices.Models.Customer;

namespace AspNetWebApiPractices.Services.Customer
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;
        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public CustomerDto GetCustomerById(int customerId)
        {
            return _context.Customers.Where(c=>c.Id == customerId).Select(c => new CustomerDto
            {
                Id = c.Id,
                FullName = c.FullName
            }).SingleOrDefault();
        }
    }
}
