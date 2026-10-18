using Microsoft.Extensions.DependencyInjection;

namespace MyScheduling.DependencyInjection.Queries.Agendamentos;

public static class QueriesConfiguration
{
    public static IServiceCollection ConfigureQueries(this IServiceCollection services)
    {
        AgendamentoQueriesConfiguration.ConfigureQueries(services);

        return services;
    }
}
