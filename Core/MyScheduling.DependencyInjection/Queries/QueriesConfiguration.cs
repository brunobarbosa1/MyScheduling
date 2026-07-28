using MyScheduling.DependencyInjection.Queries.Agendamentos;
using Microsoft.Extensions.DependencyInjection;

namespace MyScheduling.DependencyInjection.Queries;

public static class QueriesConfiguration
{
    public static IServiceCollection ConfigureQueries(this IServiceCollection services)
    {
        AgendamentoQueriesConfiguration.ConfigureQueries(services);

        return services;
    }
}
