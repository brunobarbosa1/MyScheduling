using MyScheduling.Application.Agendamentos.Commands.CreateAgendamento;
using MyScheduling.Domain.Enums;
using Xunit;

namespace MyScheduling.Tests.Agendamentos;

public class CreateAgendamentoCommandHandlerTests
{
    private static readonly DateTimeOffset Agora = new(2026, 6, 3, 12, 0, 0, TimeSpan.Zero);

    private static CreateAgendamentoCommand ComandoValido() => new()
    {
        ClienteNome = "Maria",
        ClienteTelefone = "11999999999",
        Servico = "Corte",
        ValorServico = 80m,
        DataHoraInicio = Agora.AddHours(1),
        DataHoraFim = Agora.AddHours(2),
        Observacao = null
    };

    private static (CreateAgendamentoCommandHandler handler, FakeAgendamentoRepository repo) Sut(
        bool existeSobreposicao = false)
    {
        var repo = new FakeAgendamentoRepository { ExisteSobreposicao = existeSobreposicao };
        var handler = new CreateAgendamentoCommandHandler(repo, new FixedTimeProvider(Agora));
        return (handler, repo);
    }

    [Fact]
    public async Task Handle_ComandoValido_CriaAgendamentoComStatusAgendado()
    {
        // Arrange
        var (handler, repo) = Sut();

        // Act
        var result = await handler.Handle(ComandoValido());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(nameof(StatusAgendamento.Agendado), result.Value.Status);
        Assert.Single(repo.Salvos);
        Assert.Equal(1, repo.SaveCount);
    }

    [Fact]
    public async Task Handle_DataHoraInicioNoPassado_RetornaFalhaSemPersistir()
    {
        // Arrange — RN003
        var (handler, repo) = Sut();
        var command = ComandoValido() with { DataHoraInicio = Agora.AddHours(-1) };

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("agendamento.passado", result.Error.Code);
        Assert.Empty(repo.Salvos);
    }

    [Fact]
    public async Task Handle_HorarioSobreposto_RetornaConflito()
    {
        // Arrange — RN001
        var (handler, repo) = Sut(existeSobreposicao: true);

        // Act
        var result = await handler.Handle(ComandoValido());

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("agendamento.sobreposicao", result.Error.Code);
        Assert.Empty(repo.Salvos);
    }

    [Fact]
    public async Task Handle_DataHoraFimMenorOuIgualInicio_RetornaValidacao()
    {
        // Arrange — RN002
        var (handler, _) = Sut();
        var command = ComandoValido() with { DataHoraFim = ComandoValido().DataHoraInicio };

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("agendamento.validacao", result.Error.Code);
    }

    [Fact]
    public async Task Handle_ClienteNomeVazio_RetornaValidacao()
    {
        // Arrange — RN005
        var (handler, _) = Sut();
        var command = ComandoValido() with { ClienteNome = "" };

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("agendamento.validacao", result.Error.Code);
    }

    [Fact]
    public async Task Handle_ValorServicoZeroOuNegativo_RetornaValidacao()
    {
        // Arrange — RN006
        var (handler, _) = Sut();
        var command = ComandoValido() with { ValorServico = 0m };

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("agendamento.validacao", result.Error.Code);
    }
}
