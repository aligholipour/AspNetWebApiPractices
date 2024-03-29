using AspNetWebApiPractices.Extensions;
using AspNetWebApiPractices.Helpers;
using AspNetWebApiPractices.Infrastructures;
using AspNetWebApiPractices.Infrastructures.Swagger;
using AspNetWebApiPractices.Services.Customers;
using AspNetWebApiPractices.Services.Files;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading.RateLimiting;
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
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddApiVersioning(config =>
{
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSwaggerGen(options =>
{
    var xmlfilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlfilename));
});

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddCustomRateLimiter();

var app = builder.Build();

app.UseStaticFiles();

app.ConfigureExceptionHandler();

app.MapControllers();

app.UseRateLimiter();

var apiVersionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    foreach (var description in apiVersionProvider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    }
});


app.Run();