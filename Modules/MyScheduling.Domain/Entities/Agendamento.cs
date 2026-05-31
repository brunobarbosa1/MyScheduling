using MyScheduling.Common.Results;
using MyScheduling.Domain.Enums;

namespace MyScheduling.Domain.Entities;

public class Agendamento : Entity
{
    public string ClienteNome { get; private set; }
    public string? ClienteTelefone { get; private set; }
    public string Servico { get; private set; }
    public decimal ValorServico { get; private set; }
    public DateTime DataHoraInicio { get; private set; }
    public DateTime DataHoraFim { get; private set; }
    public StatusAgendamento Status { get; private set; }
    public string? Observacao { get; private set; }

    private Agendamento()
    {
        ClienteNome = null!;
        Servico = null!;
    }

    private Agendamento(
        string clienteNome,
        string? clienteTelefone,
        string servico,
        decimal valorServico,
        DateTime dataHoraInicio,
        DateTime dataHoraFim,
        string? observacao)
    {
        ClienteNome = clienteNome;
        ClienteTelefone = clienteTelefone;
        Servico = servico;
        ValorServico = valorServico;
        DataHoraInicio = dataHoraInicio;
        DataHoraFim = dataHoraFim;
        Observacao = observacao;
        Status = StatusAgendamento.Agendado;
    }

    public static Result<Agendamento> Criar(
        string clienteNome,
        string? clienteTelefone,
        string servico,
        decimal valorServico,
        DateTime dataHoraInicio,
        DateTime dataHoraFim,
        string? observacao,
        DateTime agora)
    {
        var validacao = ValidarInvariantes(clienteNome, servico, valorServico, dataHoraInicio, dataHoraFim, agora);
        if (validacao.IsFailure)
            return Result.Failure<Agendamento>(validacao.Error);

        var agendamento = new Agendamento(
            clienteNome.Trim(),
            clienteTelefone?.Trim(),
            servico.Trim(),
            valorServico,
            dataHoraInicio,
            dataHoraFim,
            observacao?.Trim());

        return Result.Success(agendamento);
    }

    public Result Atualizar(
        string clienteNome,
        string? clienteTelefone,
        string servico,
        decimal valorServico,
        DateTime dataHoraInicio,
        DateTime dataHoraFim,
        string? observacao,
        DateTime agora)
    {
        if (Status != StatusAgendamento.Agendado)
            return Result.Failure(new Error(
                "Agendamento.NaoEditavel",
                "Apenas agendamentos com status Agendado podem ser editados."));

        var validacao = ValidarInvariantes(clienteNome, servico, valorServico, dataHoraInicio, dataHoraFim, agora);
        if (validacao.IsFailure)
            return validacao;

        ClienteNome = clienteNome.Trim();
        ClienteTelefone = clienteTelefone?.Trim();
        Servico = servico.Trim();
        ValorServico = valorServico;
        DataHoraInicio = dataHoraInicio;
        DataHoraFim = dataHoraFim;
        Observacao = observacao?.Trim();
        MarcarComoAtualizado();

        return Result.Success();
    }

    public Result Cancelar()
    {
        if (Status == StatusAgendamento.Cancelado)
            return Result.Failure(new Error(
                "Agendamento.JaCancelado",
                "Agendamento já está cancelado."));

        if (Status == StatusAgendamento.Concluido)
            return Result.Failure(new Error(
                "Agendamento.JaConcluido",
                "Não é possível cancelar um agendamento já concluído."));

        Status = StatusAgendamento.Cancelado;
        MarcarComoAtualizado();
        return Result.Success();
    }

    public Result Concluir()
    {
        if (Status == StatusAgendamento.Concluido)
            return Result.Failure(new Error(
                "Agendamento.JaConcluido",
                "Agendamento já está concluído."));

        if (Status == StatusAgendamento.Cancelado)
            return Result.Failure(new Error(
                "Agendamento.Cancelado",
                "Não é possível concluir um agendamento cancelado."));

        Status = StatusAgendamento.Concluido;
        MarcarComoAtualizado();
        return Result.Success();
    }

    private static Result ValidarInvariantes(
        string clienteNome,
        string servico,
        decimal valorServico,
        DateTime dataHoraInicio,
        DateTime dataHoraFim,
        DateTime agora)
    {
        if (string.IsNullOrWhiteSpace(clienteNome))
            return Result.Failure(new Error(
                "Agendamento.ClienteNomeObrigatorio",
                "ClienteNome é obrigatório."));

        if (string.IsNullOrWhiteSpace(servico))
            return Result.Failure(new Error(
                "Agendamento.ServicoObrigatorio",
                "Servico é obrigatório."));

        if (valorServico <= 0)
            return Result.Failure(new Error(
                "Agendamento.ValorInvalido",
                "ValorServico deve ser maior que zero."));

        if (dataHoraFim <= dataHoraInicio)
            return Result.Failure(new Error(
                "Agendamento.HorarioInvalido",
                "DataHoraFim deve ser maior que DataHoraInicio."));

        if (dataHoraInicio < agora)
            return Result.Failure(new Error(
                "Agendamento.HorarioNoPassado",
                "Não é permitido criar agendamento no passado."));

        return Result.Success();
    }
}
