using Microsoft.EntityFrameworkCore;
using MyScheduling.Application.DependencyInjection;
using MyScheduling.Data.Contexts;
using MyScheduling.Data.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Postgres")
    ?? throw new InvalidOperationException(
        "Connection string 'Postgres' não configurada. Defina ConnectionStrings__Postgres (env) ou ConnectionStrings:Postgres (appsettings).");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .ConfigureData(connectionString)
    .ConfigureApplication();

var app = builder.Build();

// Aplica migrations pendentes no startup (conveniência para Docker/VPS).
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AgendamentoContext>();
    context.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
