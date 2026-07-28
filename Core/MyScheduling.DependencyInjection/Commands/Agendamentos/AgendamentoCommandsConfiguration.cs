using Microsoft.Extensions.DependencyInjection;
using MyScheduling.Application.CommandHandlers.Agendamentos;
using MyScheduling.Application.Commands.Agendamentos;

namespace MyScheduling.DependencyInjection.Commands.Agendamentos;

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
