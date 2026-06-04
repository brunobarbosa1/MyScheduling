using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyScheduling.Data.Contexts;

/// <summary>
/// Fábrica usada apenas em design-time (dotnet ef migrations/database).
/// Lê a connection string da variável de ambiente ConnectionStrings__Postgres;
/// na ausência dela, usa um valor local padrão (não é necessário um banco ativo
/// para gerar migrations).
/// </summary>
public sealed class AgendamentoContextFactory : IDesignTimeDbContextFactory<AgendamentoContext>
{
    public AgendamentoContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__Postgres")
            ?? "Host=localhost;Port=5432;Database=myscheduling;Username=myscheduling;Password=postgres";

        var options = new DbContextOptionsBuilder<AgendamentoContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new AgendamentoContext(options);
    }
}
