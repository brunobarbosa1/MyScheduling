using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyScheduling.Data.Contexts;
using MyScheduling.Data.Repositories;
using MyScheduling.Domain.Repositories;

namespace MyScheduling.Data.DependencyInjection;

public static class DataConfiguration
{
    public static IServiceCollection ConfigureData(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AgendamentoContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();

        return services;
    }
}
