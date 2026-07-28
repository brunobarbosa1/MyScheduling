using MyScheduling.Application.Commands.Agendamentos;
using MyScheduling.Common.Results;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.CommandHandlers.Agendamentos;

public interface IAtualizarAgendamentoCommandHandler
{
    Task<Result<AgendamentoViewModel>> Handle(
        AtualizarAgendamentoCommand command,
        CancellationToken cancellationToken = default);
}
