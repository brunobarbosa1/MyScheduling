using MyScheduling.Common.Results;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.Agendamentos.Commands.CreateAgendamento;

public interface ICreateAgendamentoCommandHandler
{
    Task<Result<AgendamentoViewModel>> Handle(
        CreateAgendamentoCommand command,
        CancellationToken cancellationToken = default);
}
