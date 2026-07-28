using MyScheduling.Common.Results;
using MyScheduling.Application.Filters.Agendamentos;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.Queries.Agendamentos;

public interface IListarAgendamentosPorDataQuery
{
    Task<Result<IReadOnlyList<AgendamentoViewModel>>> Handle(
        ListarAgendamentosPorDataFilter filter,
        CancellationToken cancellationToken = default);
}
