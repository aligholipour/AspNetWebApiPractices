using AspNetWebApiPractices.Extensions;
using AspNetWebApiPractices.Infrastructures;
using AspNetWebApiPractices.Services.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using static AspNetWebApiPractices.Extensions.FormatterExtensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers(setup =>
{
    setup.ReturnHttpNotAcceptable = true;
    setup.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
})
.AddXmlDataContractSerializerFormatters()
.ConfigureApiBehaviorOptions(setup =>
{
    setup.InvalidModelStateResponseFactory = context =>
    {
        var problemDetailsFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
        var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext, context.ModelState);

        problemDetails.Instance = context.HttpContext.Request.Path;

        var actionExecutedContext = context as ActionExecutingContext;

        if (context.ModelState.ErrorCount > 0 && (actionExecutedContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count))
        {
            problemDetails.Type = "https://courselibrary.com/modelvalidationproblem";
            problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
            problemDetails.Title = "One or more validation errors occurred.";
            problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

            return new UnprocessableEntityObjectResult(problemDetails)
            {
                ContentTypes = { "application/problem+json" }
            };
        }

        return new BadRequestObjectResult(problemDetails)
        {
            ContentTypes = { "application/problem+json" }
        };
    };
});

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.ConfigureExceptionHandler();

app.MapControllers();

app.Run();