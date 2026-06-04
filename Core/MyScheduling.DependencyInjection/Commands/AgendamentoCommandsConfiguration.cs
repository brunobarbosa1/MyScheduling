using Microsoft.Extensions.DependencyInjection;
using MyScheduling.Application.Commands;

namespace MyScheduling.DependencyInjection.Commands;

internal static class AgendamentoCommandsConfiguration
{
    public static IServiceCollection ConfigureCommands(IServiceCollection services)
    {
        services.AddScoped<ICriarAgendamentoCommandHandler, CriarAgendamentoCommandHandler>();
        services.AddScoped<IAtualizarAgendamentoCommandHandler, AtualizarAgendamentoCommandHandler>();
        services.AddScoped<ICancelarAgendamentoCommandHandler, CancelarAgendamentoCommandHandler>();
        services.AddScoped<IConcluirAgendamentoCommandHandler, ConcluirAgendamentoCommandHandler>();
        services.AddScoped<IExcluirAgendamentoCommandHandler, ExcluirAgendamentoCommandHandler>();

        return services;
    }
}
