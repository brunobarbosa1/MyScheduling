using MyScheduling.Application.Commands.Agendamentos;
using MyScheduling.Common.Results;

namespace MyScheduling.Application.CommandHandlers.Agendamentos;

public interface IExcluirAgendamentoCommandHandler
{
    Task<Result> Handle(
        ExcluirAgendamentoCommand command,
        CancellationToken cancellationToken = default);
}
