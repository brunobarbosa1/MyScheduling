using MyScheduling.Application.Commands.Agendamentos;
using MyScheduling.Common.Results;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.CommandHandlers.Agendamentos;

public interface IConcluirAgendamentoCommandHandler
{
    Task<Result<AgendamentoViewModel>> Handle(
        ConcluirAgendamentoCommand command,
        CancellationToken cancellationToken = default);
}
