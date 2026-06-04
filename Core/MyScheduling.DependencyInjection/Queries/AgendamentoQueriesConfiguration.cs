using Microsoft.Extensions.DependencyInjection;
using MyScheduling.Application.Queries;

namespace MyScheduling.DependencyInjection.Queries;

internal static class AgendamentoQueriesConfiguration
{
    public static IServiceCollection ConfigureQueries(IServiceCollection services)
    {
        services.AddScoped<IObterAgendamentoPorIdQuery, ObterAgendamentoPorIdQuery>();
        services.AddScoped<IListarAgendamentosQuery, ListarAgendamentosQuery>();
        services.AddScoped<IListarAgendamentosPorDataQuery, ListarAgendamentosPorDataQuery>();

        return services;
    }
}
