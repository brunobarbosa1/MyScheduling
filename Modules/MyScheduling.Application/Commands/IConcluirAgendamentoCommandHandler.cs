using MyScheduling.Common.Results;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.Agendamentos.Commands;

public interface IConcluirAgendamentoCommandHandler
{
    Task<Result<AgendamentoViewModel>> Handle(
        ConcluirAgendamentoCommand command,
        CancellationToken cancellationToken = default);
}
