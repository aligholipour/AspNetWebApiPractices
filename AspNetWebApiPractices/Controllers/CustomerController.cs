using Microsoft.AspNetCore.Mvc;

namespace AspNetWebApiPractices.Controllers
{
    [ApiController]
    public class CustomerController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
