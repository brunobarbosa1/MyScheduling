using MyScheduling.Common.Results;

namespace MyScheduling.Application.Agendamentos.Commands;

public interface IExcluirAgendamentoCommandHandler
{
    Task<Result> Handle(
        ExcluirAgendamentoCommand command,
        CancellationToken cancellationToken = default);
}
