using MyScheduling.Common.Results;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.Agendamentos.Queries;

public interface IObterAgendamentoPorIdQuery
{
    Task<Result<AgendamentoViewModel>> Handle(
        ObterAgendamentoPorIdFilter filter,
        CancellationToken cancellationToken = default);
}
