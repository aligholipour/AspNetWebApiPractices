namespace AspNetWebApiPractices.Domain.Customer
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PictureName { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
