using MyScheduling.Common.Results;
using MyScheduling.Domain.Entities;
using MyScheduling.Domain.Repositories;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.Commands;

public sealed class CriarAgendamentoCommandHandler : ICriarAgendamentoCommandHandler
{
    private readonly IAgendamentoRepository _agendamentoRepository;
    private readonly TimeProvider _timeProvider;

    public CriarAgendamentoCommandHandler(
        IAgendamentoRepository agendamentoRepository,
        TimeProvider timeProvider)
    {
        _agendamentoRepository = agendamentoRepository;
        _timeProvider = timeProvider;
    }

    public async Task<Result<AgendamentoViewModel>> Handle(
        CriarAgendamentoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validation = command.Validate();
        if (!validation.IsValid)
        {
            var mensagem = string.Join(" ", validation.Errors.Select(e => e.ErrorMessage));
            return Result.Failure<AgendamentoViewModel>(
                Error.Validation("agendamento.validacao", mensagem));
        }

        
        if (command.DataHoraInicio < _timeProvider.GetUtcNow())
            return Result.Failure<AgendamentoViewModel>(
                Error.Validation("agendamento.passado", "Não é permitido criar agendamentos no passado."));
        
        var existeSobreposicao = await _agendamentoRepository.ExisteSobreposicaoAsync(
            command.DataHoraInicio, command.DataHoraFim, cancellationToken: cancellationToken);

        if (existeSobreposicao)
            return Result.Failure<AgendamentoViewModel>(
                Error.Conflict("agendamento.sobreposicao", "Já existe um agendamento nesse horário."));

        var agendamento = Agendamento.Factory.CriarNovo(
            command.ClienteNome,
            command.ClienteTelefone,
            command.Servico,
            command.ValorServico,
            command.DataHoraInicio,
            command.DataHoraFim,
            command.TipoPagamento,
            command.Observacao);

        _agendamentoRepository.Add(agendamento);
        await _agendamentoRepository.SaveAsync(cancellationToken);

        return Result.Success<AgendamentoViewModel>(agendamento);
    }
}
