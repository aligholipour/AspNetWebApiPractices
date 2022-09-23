using AspNetWebApiPractices.Infrastructures;
using AspNetWebApiPractices.Services.Customer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers(setup =>
{
    setup.ReturnHttpNotAcceptable = true;

}).AddXmlDataContractSerializerFormatters();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapControllers();

app.Run();
