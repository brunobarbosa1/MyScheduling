using MyScheduling.Domain.Entities;
using MyScheduling.Domain.Enums;

namespace MyScheduling.Presentations.Agendamentos;

public sealed class AgendamentoViewModel
{
    public Guid Id { get; init; }
    public string ClienteNome { get; init; } = string.Empty;
    public string? ClienteTelefone { get; init; }
    public string Servico { get; init; } = string.Empty;
    public decimal ValorServico { get; init; }
    public DateTimeOffset DataHoraInicio { get; init; }
    public DateTimeOffset DataHoraFim { get; init; }
    public TipoPagamento? TipoPagamento { get; init; }
    public StatusAgendamento Status { get; init; }
    public string? Observacao { get; init; }
    public DateTime CriadoEm { get; init; }
    public DateTime? AtualizadoEm { get; init; }

    public static implicit operator AgendamentoViewModel(Agendamento agendamento)
        => new()
        {
            Id = agendamento.Id,
            ClienteNome = agendamento.ClienteNome,
            ClienteTelefone = agendamento.ClienteTelefone,
            Servico = agendamento.Servico,
            ValorServico = agendamento.ValorServico,
            DataHoraInicio = agendamento.DataHoraInicio,
            DataHoraFim = agendamento.DataHoraFim,
            TipoPagamento = agendamento.TipoPagamento,
            Status = agendamento.Status,
            Observacao = agendamento.Observacao,
            CriadoEm = agendamento.CriadoEm,
            AtualizadoEm = agendamento.AtualizadoEm
        };
}
