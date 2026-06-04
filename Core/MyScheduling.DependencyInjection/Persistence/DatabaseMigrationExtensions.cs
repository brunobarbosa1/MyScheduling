using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyScheduling.Data.Contexts;

namespace MyScheduling.DependencyInjection.Persistence;

public static class DatabaseMigrationExtensions
{
    /// <summary>
    /// Aplica as migrations pendentes no startup (conveniência para Docker/VPS).
    /// </summary>
    public static void MigrateDatabase(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AgendamentoContext>();
        context.Database.Migrate();
    }
}
