using AspNetWebApiPractices.Models.Customer;

namespace AspNetWebApiPractices.Services.Customer
{
    public interface ICustomerRepository
    {
        CustomerDto GetCustomerById(int customerId);
    }
}
