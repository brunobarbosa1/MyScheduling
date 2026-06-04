using MyScheduling.Common.Results;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.Agendamentos.Queries;

public interface IListarAgendamentosQuery
{
    Task<Result<IReadOnlyList<AgendamentoViewModel>>> Handle(
        ListarAgendamentosFilter filter,
        CancellationToken cancellationToken = default);
}
