using MyScheduling.Common.Results;
using MyScheduling.Application.Filters.Agendamentos;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.Queries.Agendamentos;

public interface IListarAgendamentosQuery
{
    Task<Result<IReadOnlyList<AgendamentoViewModel>>> Handle(
        ListarAgendamentosFilter filter,
        CancellationToken cancellationToken = default);
}
