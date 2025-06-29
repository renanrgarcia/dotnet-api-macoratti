//Approach using WebApplicationBuilder
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApiPlayground.Clients;
using WebApiPlayground.Services;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddScoped<NumbersService>();

    //ServiceDescriptor serviceDescriptor = new(
    //    typeof(INumbersClient),
    //    typeof(NumberThreeClient),
    //    ServiceLifetime.Scoped);

    //builder.Services.Add(serviceDescriptor);

    //builder.Services.TryAddScoped<INumbersClient, NumberThreeClient>();
    //builder.Services.TryAddScoped<INumbersClient, NumberFiveClient>();

    builder.Services.TryAddScoped<INumbersClient, NumberThreeClient>();
    builder.Services.TryAddScoped<INumbersClient, NumberFiveClient>();

    // Adds Controllers services to the DI container
    builder.Services.AddControllers();
}

var app = builder.Build();
{
    // Configure the Controllers middleware
    app.MapControllers();

    //app.Use(async (httpContext, next) =>
    //{
    //    var client = httpContext.RequestServices.GetRequiredService<NumberFiveClient>();

    //    client.IncrementNumber();

    //    await next();
    //});

    app.Run();
}


//// Approach using ServiceCollection and ServiceProvider
//using Dumpify;
//using WebApiPlayground.Controllers;
//using WebApiPlayground.Services;

//ServiceCollection services = [];

//services.AddTransient<NumbersController>();
//services.AddTransient<NumbersService>();

//ServiceProvider serviceProvider = services.BuildServiceProvider();

//var controller = serviceProvider.GetRequiredService<NumbersController>();

//controller.GetNumber().Dump();