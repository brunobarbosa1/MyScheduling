using Microsoft.Extensions.DependencyInjection;
using MyScheduling.Data.Repositories;
using MyScheduling.Domain.Repositories;

namespace MyScheduling.DependencyInjection.Persistence;

public static class RepositoriesConfiguration
{
    public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        return services;
    }
}
