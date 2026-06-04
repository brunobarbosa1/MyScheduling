using MyScheduling.Domain.Enums;

namespace MyScheduling.Domain.Entities;

public class Agendamento : Entity
{
    protected Agendamento() { }
    public string ClienteNome { get; private set; } = null!;
    public string? ClienteTelefone { get; private set; }
    public string Servico { get; private set; } = null!;
    public decimal ValorServico { get; private set; }
    public DateTimeOffset DataHoraInicio { get; private set; }
    public DateTimeOffset DataHoraFim { get; private set; }
    public StatusAgendamento Status { get; private set; }
    public string? Observacao { get; private set; }


    public static class Factory
    {
        public static Agendamento CriarNovo(
            string clienteNome,
            string? clienteTelefone,
            string servico,
            decimal valorServico,
            DateTimeOffset dataHoraInicio,
            DateTimeOffset dataHoraFim,
            string? observacao
        )
        {
            return new Agendamento()
            {
                ClienteNome = clienteNome,
                ClienteTelefone = clienteTelefone,
                Servico = servico,
                ValorServico = valorServico,
                DataHoraInicio = dataHoraInicio,
                DataHoraFim = dataHoraFim,
                Observacao = observacao,
                Status = StatusAgendamento.Agendado
            };
        }
    }

    public void Atualizar(
        string clienteNome,
        string? clienteTelefone,
        string servico,
        decimal valorServico,
        DateTimeOffset dataHoraInicio,
        DateTimeOffset dataHoraFim,
        string? observacao)
    {
        ClienteNome = clienteNome;
        ClienteTelefone = clienteTelefone;
        Servico = servico;
        ValorServico = valorServico;
        DataHoraInicio = dataHoraInicio;
        DataHoraFim = dataHoraFim;
        Observacao = observacao;
        MarcarComoAtualizado();
    }

    public void Cancelar()
    {
        Status = StatusAgendamento.Cancelado;
        MarcarComoAtualizado();
    }

    public void Concluir()
    {
        Status = StatusAgendamento.Concluido;
        MarcarComoAtualizado();
    }
}
