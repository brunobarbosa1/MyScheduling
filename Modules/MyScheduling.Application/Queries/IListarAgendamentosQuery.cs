using MyScheduling.Common.Results;
using MyScheduling.Application.Filters;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.Queries;

public interface IListarAgendamentosQuery
{
    Task<Result<IReadOnlyList<AgendamentoViewModel>>> Handle(
        ListarAgendamentosFilter filter,
        CancellationToken cancellationToken = default);
}
