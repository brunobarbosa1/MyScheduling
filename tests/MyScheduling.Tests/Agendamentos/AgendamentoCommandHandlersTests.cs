using MyScheduling.Application.Commands;
using MyScheduling.Domain.Entities;
using MyScheduling.Domain.Enums;
using Xunit;

namespace MyScheduling.Tests.Agendamentos;

public class AgendamentoCommandHandlersTests
{
    private static readonly DateTimeOffset Agora = new(2026, 6, 3, 12, 0, 0, TimeSpan.Zero);

    private static CriarAgendamentoCommand ComandoCriarValido() => new()
    {
        ClienteNome = "Maria",
        ClienteTelefone = "11999999999",
        Servico = "Corte",
        ValorServico = 80m,
        DataHoraInicio = Agora.AddHours(1),
        DataHoraFim = Agora.AddHours(2),
        Observacao = null
    };

    private static Agendamento NovoAgendamento() => Agendamento.Factory.CriarNovo(
        clienteNome: "Maria",
        clienteTelefone: "11999999999",
        servico: "Corte",
        valorServico: 80m,
        dataHoraInicio: Agora.AddHours(1),
        dataHoraFim: Agora.AddHours(2),
        observacao: null);

    // ----- Criar -----

    [Fact]
    public async Task Criar_ComandoValido_CriaAgendamentoComStatusAgendado()
    {
        var repo = new FakeAgendamentoRepository();
        var handler = new CriarAgendamentoCommandHandler(repo, new FixedTimeProvider(Agora));

        var result = await handler.Handle(ComandoCriarValido());

        Assert.True(result.IsSuccess);
        Assert.Equal(StatusAgendamento.Agendado, result.Value.Status);
        Assert.Single(repo.Salvos);
        Assert.Equal(1, repo.SaveCount);
    }

    [Fact]
    public async Task Criar_DataHoraInicioNoPassado_RetornaFalhaSemPersistir()
    {
        // RN003
        var repo = new FakeAgendamentoRepository();
        var handler = new CriarAgendamentoCommandHandler(repo, new FixedTimeProvider(Agora));
        var command = ComandoCriarValido() with { DataHoraInicio = Agora.AddHours(-1) };

        var result = await handler.Handle(command);

        Assert.True(result.IsFailure);
        Assert.Equal("agendamento.passado", result.Error.Code);
        Assert.Empty(repo.Salvos);
    }

    [Fact]
    public async Task Criar_HorarioSobreposto_RetornaConflito()
    {
        // RN001
        var repo = new FakeAgendamentoRepository { ExisteSobreposicao = true };
        var handler = new CriarAgendamentoCommandHandler(repo, new FixedTimeProvider(Agora));

        var result = await handler.Handle(ComandoCriarValido());

        Assert.True(result.IsFailure);
        Assert.Equal("agendamento.sobreposicao", result.Error.Code);
        Assert.Empty(repo.Salvos);
    }

    [Fact]
    public async Task Criar_DataHoraFimMenorOuIgualInicio_RetornaValidacao()
    {
        // RN002
        var repo = new FakeAgendamentoRepository();
        var handler = new CriarAgendamentoCommandHandler(repo, new FixedTimeProvider(Agora));
        var command = ComandoCriarValido() with { DataHoraFim = ComandoCriarValido().DataHoraInicio };

        var result = await handler.Handle(command);

        Assert.True(result.IsFailure);
        Assert.Equal("agendamento.validacao", result.Error.Code);
    }

    [Fact]
    public async Task Criar_ClienteNomeVazio_RetornaValidacao()
    {
        // RN005
        var repo = new FakeAgendamentoRepository();
        var handler = new CriarAgendamentoCommandHandler(repo, new FixedTimeProvider(Agora));
        var command = ComandoCriarValido() with { ClienteNome = "" };

        var result = await handler.Handle(command);

        Assert.True(result.IsFailure);
        Assert.Equal("agendamento.validacao", result.Error.Code);
    }

    [Fact]
    public async Task Criar_ValorServicoZeroOuNegativo_RetornaValidacao()
    {
        // RN006
        var repo = new FakeAgendamentoRepository();
        var handler = new CriarAgendamentoCommandHandler(repo, new FixedTimeProvider(Agora));
        var command = ComandoCriarValido() with { ValorServico = 0m };

        var result = await handler.Handle(command);

        Assert.True(result.IsFailure);
        Assert.Equal("agendamento.validacao", result.Error.Code);
    }

    // ----- Atualizar -----

    [Fact]
    public async Task Atualizar_AgendamentoInexistente_RetornaNotFound()
    {
        var repo = new FakeAgendamentoRepository();
        var handler = new AtualizarAgendamentoCommandHandler(repo, new FixedTimeProvider(Agora));

        var result = await handler.Handle(new AtualizarAgendamentoCommand
        {
            Id = Guid.NewGuid(),
            ClienteNome = "Ana",
            Servico = "Escova",
            ValorServico = 100m,
            DataHoraInicio = Agora.AddHours(3),
            DataHoraFim = Agora.AddHours(4)
        });

        Assert.True(result.IsFailure);
        Assert.Equal("agendamento.naoEncontrado", result.Error.Code);
    }

    [Fact]
    public async Task Atualizar_ComandoValido_AtualizaCampos()
    {
        var agendamento = NovoAgendamento();
        var repo = new FakeAgendamentoRepository();
        repo.Salvos.Add(agendamento);
        var handler = new AtualizarAgendamentoCommandHandler(repo, new FixedTimeProvider(Agora));

        var result = await handler.Handle(new AtualizarAgendamentoCommand
        {
            Id = agendamento.Id,
            ClienteNome = "Ana Paula",
            Servico = "Escova",
            ValorServico = 150m,
            DataHoraInicio = Agora.AddHours(5),
            DataHoraFim = Agora.AddHours(6),
            Observacao = "Remarcado"
        });

        Assert.True(result.IsSuccess);
        Assert.Equal("Ana Paula", result.Value.ClienteNome);
        Assert.Equal(150m, result.Value.ValorServico);
        Assert.Equal(1, repo.SaveCount);
    }

    [Fact]
    public async Task Atualizar_HorarioSobreposto_RetornaConflito()
    {
        var agendamento = NovoAgendamento();
        var repo = new FakeAgendamentoRepository { ExisteSobreposicao = true };
        repo.Salvos.Add(agendamento);
        var handler = new AtualizarAgendamentoCommandHandler(repo, new FixedTimeProvider(Agora));

        var result = await handler.Handle(new AtualizarAgendamentoCommand
        {
            Id = agendamento.Id,
            ClienteNome = "Maria",
            Servico = "Corte",
            ValorServico = 80m,
            DataHoraInicio = Agora.AddHours(1),
            DataHoraFim = Agora.AddHours(2)
        });

        Assert.True(result.IsFailure);
        Assert.Equal("agendamento.sobreposicao", result.Error.Code);
    }

    // ----- Cancelar -----

    [Fact]
    public async Task Cancelar_ComandoValido_AlteraStatusParaCancelado()
    {
        var agendamento = NovoAgendamento();
        var repo = new FakeAgendamentoRepository();
        repo.Salvos.Add(agendamento);
        var handler = new CancelarAgendamentoCommandHandler(repo);

        var result = await handler.Handle(new CancelarAgendamentoCommand { Id = agendamento.Id });

        Assert.True(result.IsSuccess);
        Assert.Equal(StatusAgendamento.Cancelado, result.Value.Status);
    }

    [Fact]
    public async Task Cancelar_AgendamentoInexistente_RetornaNotFound()
    {
        var repo = new FakeAgendamentoRepository();
        var handler = new CancelarAgendamentoCommandHandler(repo);

        var result = await handler.Handle(new CancelarAgendamentoCommand { Id = Guid.NewGuid() });

        Assert.True(result.IsFailure);
        Assert.Equal("agendamento.naoEncontrado", result.Error.Code);
    }

    // ----- Concluir -----

    [Fact]
    public async Task Concluir_ComandoValido_AlteraStatusParaConcluido()
    {
        var agendamento = NovoAgendamento();
        var repo = new FakeAgendamentoRepository();
        repo.Salvos.Add(agendamento);
        var handler = new ConcluirAgendamentoCommandHandler(repo);

        var result = await handler.Handle(new ConcluirAgendamentoCommand { Id = agendamento.Id });

        Assert.True(result.IsSuccess);
        Assert.Equal(StatusAgendamento.Concluido, result.Value.Status);
    }

    // ----- Excluir -----

    [Fact]
    public async Task Excluir_ComandoValido_RemoveAgendamento()
    {
        var agendamento = NovoAgendamento();
        var repo = new FakeAgendamentoRepository();
        repo.Salvos.Add(agendamento);
        var handler = new ExcluirAgendamentoCommandHandler(repo);

        var result = await handler.Handle(new ExcluirAgendamentoCommand { Id = agendamento.Id });

        Assert.True(result.IsSuccess);
        Assert.Empty(repo.Salvos);
        Assert.Equal(1, repo.SaveCount);
    }

    [Fact]
    public async Task Excluir_AgendamentoInexistente_RetornaNotFound()
    {
        var repo = new FakeAgendamentoRepository();
        var handler = new ExcluirAgendamentoCommandHandler(repo);

        var result = await handler.Handle(new ExcluirAgendamentoCommand { Id = Guid.NewGuid() });

        Assert.True(result.IsFailure);
        Assert.Equal("agendamento.naoEncontrado", result.Error.Code);
    }
}
