using AspNetWebApiPractices.Services.Customer;
using Microsoft.AspNetCore.Mvc;

namespace AspNetWebApiPractices.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet("{customerId}")]
        public IActionResult GetCustomerById(int customerId)
        {
            var customer = _customerRepository.GetCustomerById(customerId);

            if (customer is null)
                return NotFound();

            return Ok(customer);
        }
    }
}
