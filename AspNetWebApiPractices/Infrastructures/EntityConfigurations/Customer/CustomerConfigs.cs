using AspNetWebApiPractices.Domain.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetWebApiPractices.Infrastructures.EntityConfigurations.Customers
{
    public class CustomerConfigs : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(x => x.FullName).HasMaxLength(50);
            builder.Property(x => x.PictureName).HasMaxLength(100);
        }
    }
}
