using MyScheduling.Common.Results;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.Agendamentos.Commands;

public interface ICancelarAgendamentoCommandHandler
{
    Task<Result<AgendamentoViewModel>> Handle(
        CancelarAgendamentoCommand command,
        CancellationToken cancellationToken = default);
}
