using Microsoft.Extensions.DependencyInjection;

namespace MyScheduling.DependencyInjection.Commands;

public static class CommandsConfiguration
{
    public static IServiceCollection ConfigureCommands(this IServiceCollection services)
    {
        AgendamentoCommandsConfiguration.ConfigureCommands(services);

        return services;
    }
}
