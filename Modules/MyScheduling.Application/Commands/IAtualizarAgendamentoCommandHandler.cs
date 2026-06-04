using MyScheduling.Common.Results;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.Agendamentos.Commands;

public interface IAtualizarAgendamentoCommandHandler
{
    Task<Result<AgendamentoViewModel>> Handle(
        AtualizarAgendamentoCommand command,
        CancellationToken cancellationToken = default);
}
