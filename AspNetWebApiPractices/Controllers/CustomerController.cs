using AspNetWebApiPractices.Domain.Customer;
using AspNetWebApiPractices.Helpers;
using AspNetWebApiPractices.Models.Customers;
using AspNetWebApiPractices.Services.Customers;
using AspNetWebApiPractices.Services.Files;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace AspNetWebApiPractices.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/customers")]
    [ApiVersion("1.0")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IFileService _fileService;
        public CustomerController(ICustomerRepository customerRepository, IFileService fileService)
        {
            _customerRepository = customerRepository;
            _fileService = fileService;
        }

        [HttpGet("{customerId}", Name = "GetCustomer")]
        public IActionResult GetCustomerById(int customerId)
        {
            var customer = _customerRepository.GetCustomerById(customerId);

            if (customer is null)
                return NotFound();

            return Ok(customer);
        }

        /// <summary>
        /// Get all customer
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCustomers()
        {
            var customers = _customerRepository.GetCustomers();
            return Ok(customers);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer([FromForm] CreateCustomerDto createCustomerDto)
        {
            var pictureName = await _fileService.UploadFileAsync(createCustomerDto.Picture, "wwwroot/customer/pictures");

            var customer = new Customer { FullName = createCustomerDto.FullName, PictureName = pictureName };

            _customerRepository.CreateCustomer(customer);

            var customerDto = new CustomerDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                PictureName = pictureName
            };

            return CreatedAtRoute("GetCustomer", new { customerId = customer.Id }, customerDto);
        }

        [HttpPut("{customerId}")]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer(int customerId, [FromForm] UpdateCustomerDto updateCustomerDto)
        {
            var customer = _customerRepository.GetCustomerById(customerId);
            if (customer is null)
                return NotFound();

            customer.FullName = updateCustomerDto.FullName;

            if (updateCustomerDto.Picture is not null)
            {
                _fileService.DeleteFile("wwwroot/customer/pictures", customer.PictureName);
                var uploadedPictureName = await _fileService.UploadFileAsync(updateCustomerDto.Picture, "wwwroot/customer/pictures");

                customer.PictureName = uploadedPictureName;
            }

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

            _fileService.DeleteFile("wwwroot/customer/pictures", customer.PictureName);

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
