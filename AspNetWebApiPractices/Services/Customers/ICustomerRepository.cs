using AspNetWebApiPractices.Domain.Customer;
using AspNetWebApiPractices.Models.Customers;

namespace AspNetWebApiPractices.Services.Customers
{
    public interface ICustomerRepository
    {
        Customer GetCustomerById(int customerId);
        void CreateCustomer(Customer customer);
        List<CustomerDto> GetCustomers();
        void UpdateCustomer(Customer customer);
    }
}
