using MyScheduling.Common.Results;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.Agendamentos.Queries;

public interface IListarAgendamentosPorDataQuery
{
    Task<Result<IReadOnlyList<AgendamentoViewModel>>> Handle(
        ListarAgendamentosPorDataFilter filter,
        CancellationToken cancellationToken = default);
}
