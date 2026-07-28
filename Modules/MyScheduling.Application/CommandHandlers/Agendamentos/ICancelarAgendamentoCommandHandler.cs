using MyScheduling.Application.Commands.Agendamentos;
using MyScheduling.Common.Results;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.CommandHandlers.Agendamentos;

public interface ICancelarAgendamentoCommandHandler
{
    Task<Result<AgendamentoViewModel>> Handle(
        CancelarAgendamentoCommand command,
        CancellationToken cancellationToken = default);
}
