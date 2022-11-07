namespace AspNetWebApiPractices.Models.Customers
{
    public class UpdateCustomerDto
    {
        public string FullName { get; set; }
        public IFormFile Picture { get; set; }
    }
}
