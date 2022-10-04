using AspNetWebApiPractices.Domain.Customer;
using AspNetWebApiPractices.Helpers;
using AspNetWebApiPractices.Models.Customers;
using AspNetWebApiPractices.Services.Customers;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet("{customerId}", Name = "GetCustomer")]
        public IActionResult GetCustomerById(int customerId)
        {
            var customer = _customerRepository.GetCustomerById(customerId);

            if (customer is null)
                return NotFound();

            return Ok(customer);
        }

        [HttpGet]
        public IActionResult GetCustomers()
        {
            var customers = _customerRepository.GetCustomers();
            return Ok(customers);
        }

        [HttpPost]
        public ActionResult<CustomerDto> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            var customer = new Customer { FullName = createCustomerDto.FullName };

            _customerRepository.CreateCustomer(customer);

            var customerDto = new CustomerDto
            {
                Id = customer.Id,
                FullName = customer.FullName
            };

            return CreatedAtRoute("GetCustomer", new { customerId = customer.Id }, customerDto);
        }

        [HttpPut("{customerId}")]
        public ActionResult<CustomerDto> UpdateCustomer(int customerId, UpdateCustomerDto updateCustomerDto)
        {
            var customer = _customerRepository.GetCustomerById(customerId);
            if (customer is null)
                return NotFound();

            customer.FullName = updateCustomerDto.FullName;

            _customerRepository.UpdateCustomer(customer);

            return NoContent();
        }

        [HttpPatch("{customerId}")]
        public ActionResult<CustomerDto> PartiallyUpdateCustomer(int customerId, JsonPatchDocument<UpdateCustomerDto> patchUpdateCustomer)
        {
            var customer = _customerRepository.GetCustomerById(customerId);
            if (customer is null)
                return NotFound();

            var customerToPatch = new UpdateCustomerDto { FullName = customer.FullName };

            patchUpdateCustomer.ApplyTo(customerToPatch);

            customer.FullName = customerToPatch.FullName;

            _customerRepository.UpdateCustomer(customer);

            return NoContent();
        }

        [HttpDelete("{customerId}")]
        public IActionResult DeleteCustomer(int customerId)
        {
            var customer = _customerRepository.GetCustomerById(customerId);
            if (customer is null)
                return NotFound();

            _customerRepository.DeleteCustomer(customer);

            return Ok();
        }

        [HttpGet("GetCustomers/{ids}", Name = "GetCustomers")]
        public ActionResult<IList<CustomerDto>> GetCustomers([ModelBinder(BinderType = typeof(GetCollectionIdsModelBinder))] IList<int> ids)
        {
            if (ids is null)
                return BadRequest();

            var customers = _customerRepository.GetCustomersByIds(ids);

            if (ids.Count() != customers.Count())
                return NotFound();

            return Ok(customers);
        }
    }
}
