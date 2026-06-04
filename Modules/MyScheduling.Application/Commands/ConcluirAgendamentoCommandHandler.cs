using MyScheduling.Common.Results;
using MyScheduling.Domain.Repositories;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.Agendamentos.Commands;

public sealed class ConcluirAgendamentoCommandHandler : IConcluirAgendamentoCommandHandler
{
    private readonly IAgendamentoRepository _agendamentoRepository;

    public ConcluirAgendamentoCommandHandler(IAgendamentoRepository agendamentoRepository)
    {
        _agendamentoRepository = agendamentoRepository;
    }

    public async Task<Result<AgendamentoViewModel>> Handle(
        ConcluirAgendamentoCommand command,
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

        agendamento.Concluir();
        await _agendamentoRepository.SaveAsync(cancellationToken);

        return Result.Success<AgendamentoViewModel>(agendamento);
    }
}
