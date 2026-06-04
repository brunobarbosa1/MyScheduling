using MyScheduling.Common.Results;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.Agendamentos.Commands;

public interface ICriarAgendamentoCommandHandler
{
    Task<Result<AgendamentoViewModel>> Handle(
        CriarAgendamentoCommand command,
        CancellationToken cancellationToken = default);
}
