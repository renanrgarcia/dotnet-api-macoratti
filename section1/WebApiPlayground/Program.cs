//Approach using WebApplicationBuilder
using WebApiPlayground.Services;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddSingleton<NumbersService>();
    // Adds Controllers services to the DI container
    builder.Services.AddControllers();
}

var app = builder.Build();
{
    // Configure the Controllers middleware
    app.MapControllers();
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