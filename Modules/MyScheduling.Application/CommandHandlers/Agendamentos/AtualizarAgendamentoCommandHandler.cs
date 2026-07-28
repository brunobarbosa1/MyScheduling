using MyScheduling.Application.Commands.Agendamentos;
using MyScheduling.Common.Results;
using MyScheduling.Domain.Repositories.Agendamentos;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.CommandHandlers.Agendamentos;

public sealed class AtualizarAgendamentoCommandHandler : IAtualizarAgendamentoCommandHandler
{
    private readonly IAgendamentoRepository _agendamentoRepository;
    private readonly TimeProvider _timeProvider;

    public AtualizarAgendamentoCommandHandler(
        IAgendamentoRepository agendamentoRepository,
        TimeProvider timeProvider)
    {
        _agendamentoRepository = agendamentoRepository;
        _timeProvider = timeProvider;
    }

    public async Task<Result<AgendamentoViewModel>> Handle(
        AtualizarAgendamentoCommand command,
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

        // RN003 — não permitir horários no passado.
        if (command.DataHoraInicio < _timeProvider.GetUtcNow())
            return Result.Failure<AgendamentoViewModel>(
                Error.Validation("agendamento.passado", "Não é permitido agendar para o passado."));

        // RN001/RN004 — sobreposição (ignora o próprio registro e os cancelados).
        var existeSobreposicao = await _agendamentoRepository.ExisteSobreposicaoAsync(
            command.DataHoraInicio, command.DataHoraFim, command.Id, cancellationToken);

        if (existeSobreposicao)
            return Result.Failure<AgendamentoViewModel>(
                Error.Conflict("agendamento.sobreposicao", "Já existe um agendamento nesse horário."));

        agendamento.Atualizar(
            command.ClienteNome,
            command.ClienteTelefone,
            command.Servico,
            command.ValorServico,
            command.DataHoraInicio,
            command.DataHoraFim,
            command.TipoPagamento,
            command.Observacao);

        await _agendamentoRepository.SaveAsync(cancellationToken);

        return Result.Success<AgendamentoViewModel>(agendamento);
    }
}
