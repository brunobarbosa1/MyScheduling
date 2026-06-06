using MyScheduling.Common.Results;

namespace MyScheduling.Application.Commands;

public interface IExcluirAgendamentoCommandHandler
{
    Task<Result> Handle(
        ExcluirAgendamentoCommand command,
        CancellationToken cancellationToken = default);
}
