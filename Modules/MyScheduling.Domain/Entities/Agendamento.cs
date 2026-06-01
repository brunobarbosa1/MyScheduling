using MyScheduling.Domain.Enums;

namespace MyScheduling.Domain.Entities;

public class Agendamento : Entity
{
    protected Agendamento() { }
    public string ClienteNome { get; private set; }
    public string? ClienteTelefone { get; private set; }
    public string Servico { get; private set; }
    public decimal ValorServico { get; private set; }
    public DateOnly Data { get; private set; }
    public TimeOnly HoraInicio { get; private set; }
    public TimeOnly HoraFim { get; private set; }
    public StatusAgendamento Status { get; private set; }
    public string? Observacao { get; private set; }


    public static class Factory
    {
        public static Agendamento CriarNovo(
            string clienteNome,
            string? clienteTelefone,
            string servico,
            decimal valorServico,
            DateOnly data,
            TimeOnly horaInicio,
            TimeOnly horaFim,
            string? observacao
        )
        {
            return new Agendamento()
            {
                ClienteNome = clienteNome,
                ClienteTelefone = clienteTelefone,
                Servico = servico,
                ValorServico = valorServico,
                Data = data,
                HoraInicio = horaInicio,
                HoraFim = horaFim,
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
        DateOnly data,
        TimeOnly horaInicio,
        TimeOnly horaFim,
        string? observacao)
    {
        ClienteNome = clienteNome;
        ClienteTelefone = clienteTelefone;
        Servico = servico;
        ValorServico = valorServico;
        Data = data;
        HoraInicio = horaInicio;
        HoraFim = horaFim;
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
