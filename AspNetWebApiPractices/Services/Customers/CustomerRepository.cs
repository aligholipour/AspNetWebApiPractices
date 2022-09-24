using AspNetWebApiPractices.Domain.Customer;
using AspNetWebApiPractices.Infrastructures;
using AspNetWebApiPractices.Models.Customers;

namespace AspNetWebApiPractices.Services.Customers
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
            return _context.Customers.Where(c => c.Id == customerId).Select(c => new CustomerDto
            {
                Id = c.Id,
                FullName = c.FullName
            }).SingleOrDefault();
        }

        public List<CustomerDto> GetCustomers()
        {
            return _context.Customers.Select(x => new CustomerDto
            {
                Id = x.Id,
                FullName = x.FullName
            }).ToList();
        }

        public void CreateCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

    }
}
