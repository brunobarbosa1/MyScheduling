using MyScheduling.Common.Results;
using MyScheduling.Application.Filters;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.Queries;

public interface IListarAgendamentosPorDataQuery
{
    Task<Result<IReadOnlyList<AgendamentoViewModel>>> Handle(
        ListarAgendamentosPorDataFilter filter,
        CancellationToken cancellationToken = default);
}
