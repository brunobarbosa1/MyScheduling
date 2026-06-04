using MyScheduling.Common.Results;
using MyScheduling.Domain.Repositories;

namespace MyScheduling.Application.Agendamentos.Commands;

public sealed class ExcluirAgendamentoCommandHandler : IExcluirAgendamentoCommandHandler
{
    private readonly IAgendamentoRepository _agendamentoRepository;

    public ExcluirAgendamentoCommandHandler(IAgendamentoRepository agendamentoRepository)
    {
        _agendamentoRepository = agendamentoRepository;
    }

    public async Task<Result> Handle(
        ExcluirAgendamentoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validation = command.Validate();
        if (!validation.IsValid)
        {
            var mensagem = string.Join(" ", validation.Errors.Select(e => e.ErrorMessage));
            return Result.Failure(Error.Validation("agendamento.validacao", mensagem));
        }

        var agendamento = await _agendamentoRepository.GetByIdAsync(command.Id, cancellationToken);
        if (agendamento is null)
            return Result.Failure(
                Error.NotFound("agendamento.naoEncontrado", "Agendamento não encontrado."));

        _agendamentoRepository.Remove(agendamento);
        await _agendamentoRepository.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
