using AspNetWebApiPractices.Domain.Customer;
using AspNetWebApiPractices.Models.Customers;

namespace AspNetWebApiPractices.Services.Customers
{
    public interface ICustomerRepository
    {
        CustomerDto GetCustomerById(int customerId);
        void CreateCustomer(Customer customer);
        List<CustomerDto> GetCustomers();
    }
}
