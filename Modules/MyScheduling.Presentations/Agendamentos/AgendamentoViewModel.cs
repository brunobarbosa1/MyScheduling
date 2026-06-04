using MyScheduling.Domain.Entities;

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
    public string Status { get; init; } = string.Empty;
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
            Status = agendamento.Status.ToString(),
            Observacao = agendamento.Observacao,
            CriadoEm = agendamento.CriadoEm,
            AtualizadoEm = agendamento.AtualizadoEm
        };
}
