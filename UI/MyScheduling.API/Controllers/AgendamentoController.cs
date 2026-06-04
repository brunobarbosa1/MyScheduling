using Microsoft.AspNetCore.Mvc;
using MyScheduling.Application.Agendamentos.Commands;
using MyScheduling.Application.Agendamentos.Queries;

namespace MyScheduling.API.Controllers;

[Route("api/agendamentos")]
public sealed class AgendamentoController : ApiControllerBase
{
    private readonly ICriarAgendamentoCommandHandler _criarHandler;
    private readonly IAtualizarAgendamentoCommandHandler _atualizarHandler;
    private readonly ICancelarAgendamentoCommandHandler _cancelarHandler;
    private readonly IConcluirAgendamentoCommandHandler _concluirHandler;
    private readonly IExcluirAgendamentoCommandHandler _excluirHandler;
    private readonly IObterAgendamentoPorIdQuery _obterPorIdQuery;
    private readonly IListarAgendamentosQuery _listarQuery;
    private readonly IListarAgendamentosPorDataQuery _listarPorDataQuery;

    public AgendamentoController(
        ICriarAgendamentoCommandHandler criarHandler,
        IAtualizarAgendamentoCommandHandler atualizarHandler,
        ICancelarAgendamentoCommandHandler cancelarHandler,
        IConcluirAgendamentoCommandHandler concluirHandler,
        IExcluirAgendamentoCommandHandler excluirHandler,
        IObterAgendamentoPorIdQuery obterPorIdQuery,
        IListarAgendamentosQuery listarQuery,
        IListarAgendamentosPorDataQuery listarPorDataQuery)
    {
        _criarHandler = criarHandler;
        _atualizarHandler = atualizarHandler;
        _cancelarHandler = cancelarHandler;
        _concluirHandler = concluirHandler;
        _excluirHandler = excluirHandler;
        _obterPorIdQuery = obterPorIdQuery;
        _listarQuery = listarQuery;
        _listarPorDataQuery = listarPorDataQuery;
    }

    [HttpGet]
    public async Task<IActionResult> Listar(
        [FromQuery] ListarAgendamentosFilter filter,
        CancellationToken cancellationToken)
    {
        var result = await _listarQuery.Handle(filter, cancellationToken);
        return result.IsFailure ? HandleFailure(result.Error) : Ok(result.Value);
    }

    [HttpGet("data/{data}")]
    public async Task<IActionResult> ListarPorData(
        [FromRoute] ListarAgendamentosPorDataFilter filter,
        CancellationToken cancellationToken)
    {
        var result = await _listarPorDataQuery.Handle(filter, cancellationToken);
        return result.IsFailure ? HandleFailure(result.Error) : Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(
        [FromRoute] ObterAgendamentoPorIdFilter filter,
        CancellationToken cancellationToken)
    {
        var result = await _obterPorIdQuery.Handle(filter, cancellationToken);
        return result.IsFailure ? HandleFailure(result.Error) : Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(
        [FromBody] CriarAgendamentoCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _criarHandler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return HandleFailure(result.Error);

        return Created($"/api/agendamentos/{result.Value.Id}", result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(
        Guid id,
        [FromBody] AtualizarAgendamentoCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _atualizarHandler.Handle(command with { Id = id }, cancellationToken);
        return result.IsFailure ? HandleFailure(result.Error) : Ok(result.Value);
    }

    [HttpPatch("{id:guid}/cancelar")]
    public async Task<IActionResult> Cancelar(
        [FromRoute] CancelarAgendamentoCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _cancelarHandler.Handle(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result.Error) : Ok(result.Value);
    }

    [HttpPatch("{id:guid}/concluir")]
    public async Task<IActionResult> Concluir(
        [FromRoute] ConcluirAgendamentoCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _concluirHandler.Handle(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result.Error) : Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(
        [FromRoute] ExcluirAgendamentoCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _excluirHandler.Handle(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result.Error) : NoContent();
    }
}
