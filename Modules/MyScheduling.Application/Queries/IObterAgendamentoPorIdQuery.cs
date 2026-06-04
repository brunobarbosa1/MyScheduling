using MyScheduling.Common.Results;
using MyScheduling.Application.Filters;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.Queries;

public interface IObterAgendamentoPorIdQuery
{
    Task<Result<AgendamentoViewModel>> Handle(
        ObterAgendamentoPorIdFilter filter,
        CancellationToken cancellationToken = default);
}
