using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyScheduling.Application.Agendamentos.Commands.CreateAgendamento;

namespace MyScheduling.Application.DependencyInjection;

public static class ApplicationConfiguration
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        services.TryAddSingleton(TimeProvider.System);

        services.AddScoped<ICreateAgendamentoCommandHandler, CreateAgendamentoCommandHandler>();

        return services;
    }
}
