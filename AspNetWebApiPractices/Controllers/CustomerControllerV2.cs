using AspNetWebApiPractices.Models.Customers;
using AspNetWebApiPractices.Services.Customers;
using Microsoft.AspNetCore.Mvc;

namespace AspNetWebApiPractices.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/customers")]
    [ApiVersion("2.0")]
    public class CustomerControllerV2 : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCustomers()
        {
            var customers = new CustomerDto
            {
                Id= 1,
                FullName = "test fullname",
            };

            return Ok(customers);
        }
    }
}
