using System.ComponentModel.DataAnnotations;

namespace AspNetWebApiPractices.Models.Customers
{
    public class CreateCustomerDto
    {
        [Required(ErrorMessage = "The name cannot be empty")]
        [MaxLength(ErrorMessage = "The name cannot have more than 50 characters")]
        public string FullName { get; set; }
        public IFormFile Picture { get; set; }

    }
}
