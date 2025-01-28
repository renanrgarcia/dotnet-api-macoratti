using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TarefasDB"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // app.UseSwaggerUI(options =>
    //     options.SwaggerEndpoint("/openapi/v1.json", "minimalApiTarefas"));
    app.MapScalarApiReference();
}

app.MapGet("/", () => "Hello World!");

app.MapGet("frases", async () =>
    await new HttpClient().GetStringAsync("https://ron-swanson-quotes.herokuapp.com/v2/quotes")
);

app.MapGet("/tarefas", async (AppDbContext dbContext) => await dbContext.Tarefas.ToListAsync());

app.MapPost("/tarefas", async (AppDbContext dbContext, Tarefa tarefa) =>
{
    dbContext.Tarefas.Add(tarefa);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/tarefas/{tarefa.Id}", tarefa);
});

app.UseHttpsRedirection();

app.Run();

public class Tarefa
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public bool IsConcluida { get; set; }
}

class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Tarefa> Tarefas => Set<Tarefa>();
}