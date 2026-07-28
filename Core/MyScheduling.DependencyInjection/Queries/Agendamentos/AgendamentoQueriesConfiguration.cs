using Microsoft.Extensions.DependencyInjection;
using MyScheduling.Application.Queries.Agendamentos;

namespace MyScheduling.DependencyInjection.Queries.Agendamentos;

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
