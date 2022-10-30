namespace AspNetWebApiPractices.Domain.Customer
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
