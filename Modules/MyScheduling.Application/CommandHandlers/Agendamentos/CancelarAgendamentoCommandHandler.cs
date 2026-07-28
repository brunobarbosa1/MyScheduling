using MyScheduling.Application.Commands.Agendamentos;
using MyScheduling.Common.Results;
using MyScheduling.Domain.Repositories.Agendamentos;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.CommandHandlers.Agendamentos;

public sealed class CancelarAgendamentoCommandHandler : ICancelarAgendamentoCommandHandler
{
    private readonly IAgendamentoRepository _agendamentoRepository;

    public CancelarAgendamentoCommandHandler(IAgendamentoRepository agendamentoRepository)
    {
        _agendamentoRepository = agendamentoRepository;
    }

    public async Task<Result<AgendamentoViewModel>> Handle(
        CancelarAgendamentoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validation = command.Validate();
        if (!validation.IsValid)
        {
            var mensagem = string.Join(" ", validation.Errors.Select(e => e.ErrorMessage));
            return Result.Failure<AgendamentoViewModel>(
                Error.Validation("agendamento.validacao", mensagem));
        }

        var agendamento = await _agendamentoRepository.GetByIdAsync(command.Id, cancellationToken);
        if (agendamento is null)
            return Result.Failure<AgendamentoViewModel>(
                Error.NotFound("agendamento.naoEncontrado", "Agendamento não encontrado."));

        agendamento.Cancelar();
        await _agendamentoRepository.SaveAsync(cancellationToken);

        return Result.Success<AgendamentoViewModel>(agendamento);
    }
}
