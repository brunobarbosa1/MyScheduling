using MyScheduling.Common.Results;
using MyScheduling.Application.Filters.Agendamentos;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.Queries.Agendamentos;

public interface IObterAgendamentoPorIdQuery
{
    Task<Result<AgendamentoViewModel>> Handle(
        ObterAgendamentoPorIdFilter filter,
        CancellationToken cancellationToken = default);
}
