using System.Text.Json.Serialization;
using MyScheduling.DependencyInjection.Commands;
using MyScheduling.DependencyInjection.Persistence;
using MyScheduling.DependencyInjection.Queries;
using MyScheduling.DependencyInjection.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Postgres")
    ?? throw new InvalidOperationException(
        "Connection string 'Postgres' não configurada. Defina ConnectionStrings__Postgres (env) ou ConnectionStrings:Postgres (appsettings).");

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .ConfigureDbContext(connectionString)
    .ConfigureRepositories()
    .ConfigureServices()
    .ConfigureCommands()
    .ConfigureQueries();

var app = builder.Build();

app.Services.MigrateDatabase();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
