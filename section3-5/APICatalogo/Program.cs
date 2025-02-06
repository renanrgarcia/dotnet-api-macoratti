using ApiCatalogo.Context;
using ApiCatalogo.Extensions;
using ApiCatalogo.Filters;
using ApiCatalogo.Logging;
using ApiCatalogo.Repositories;
using ApiCatalogo.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiLoggingFilter));
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(mySqlConnection,
        ServerVersion.AutoDetect(mySqlConnection));
});

builder.Services.AddTransient<IMyService, MyService>();

// // Forces to use like before .net 7, forcing to use [FromServices] attribute
// builder.Services.Configure<ApiBehaviorOptions>(options =>
// {
//     options.DisableImplicitFromServicesParameters = true;
// });

builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
