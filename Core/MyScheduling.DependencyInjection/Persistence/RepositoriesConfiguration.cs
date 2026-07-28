using Microsoft.Extensions.DependencyInjection;
using MyScheduling.Data.Repositories.Agendamentos;
using MyScheduling.Data.Repositories.Usuarios;
using MyScheduling.Domain.Repositories.Agendamentos;
using MyScheduling.Domain.Repositories.Usuarios;

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
