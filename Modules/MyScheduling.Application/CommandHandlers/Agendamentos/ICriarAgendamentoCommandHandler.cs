using MyScheduling.Application.Commands.Agendamentos;
using MyScheduling.Common.Results;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.CommandHandlers.Agendamentos;

public interface ICriarAgendamentoCommandHandler
{
    Task<Result<AgendamentoViewModel>> Handle(
        CriarAgendamentoCommand command,
        CancellationToken cancellationToken = default);
}
