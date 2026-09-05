using Microsoft.AspNetCore.Mvc;
using MyScheduling.Application.Filters.Agendamentos;
using MyScheduling.Application.CommandHandlers.Agendamentos;
using MyScheduling.Application.Commands.Agendamentos;
using MyScheduling.Application.Queries.Agendamentos;

namespace MyScheduling.API.Controllers.Agendamentos;

[Route("api/v1/agendamentos")]
public sealed class AgendamentoController : ApiControllerBase
{
    private readonly ICriarAgendamentoCommandHandler _criarAgendamentoCommandHandler;
    private readonly IAtualizarAgendamentoCommandHandler _atualizarAgendamentoCommandHandler;
    private readonly ICancelarAgendamentoCommandHandler _cancelarAgendamentoCommandHandlerHandler;
    private readonly IConcluirAgendamentoCommandHandler _concluirAgendamentoCommandHandlerHandler;
    private readonly IExcluirAgendamentoCommandHandler _excluirAgendamentoCommandHandlerHandler;
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
        _criarAgendamentoCommandHandler = criarHandler;
        _atualizarAgendamentoCommandHandler = atualizarHandler;
        _cancelarAgendamentoCommandHandlerHandler = cancelarHandler;
        _concluirAgendamentoCommandHandlerHandler = concluirHandler;
        _excluirAgendamentoCommandHandlerHandler = excluirHandler;
        _obterPorIdQuery = obterPorIdQuery;
        _listarQuery = listarQuery;
        _listarPorDataQuery = listarPorDataQuery;
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] ListarAgendamentosFilter filter, CancellationToken cancellationToken)
    {
        var result = await _listarQuery.Handle(filter, cancellationToken);
        return result.IsFailure ? HandleFailure(result.Error) : Ok(result.Value);
    }

    [HttpGet("data/{data}")]
    public async Task<IActionResult> ListarPorData([FromRoute] ListarAgendamentosPorDataFilter filter, CancellationToken cancellationToken)
    {
        var result = await _listarPorDataQuery.Handle(filter, cancellationToken);
        return result.IsFailure ? HandleFailure(result.Error) : Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId([FromRoute] ObterAgendamentoPorIdFilter filter, CancellationToken cancellationToken)
    {
        var result = await _obterPorIdQuery.Handle(filter, cancellationToken);
        return result.IsFailure ? HandleFailure(result.Error) : Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarAgendamentoCommand command, CancellationToken cancellationToken)
    {
        var result = await _criarAgendamentoCommandHandler.Handle(command, cancellationToken);
        
        return result.IsFailure
            ? HandleFailure(result.Error)
            : Created($"/api/agendamentos/{result.Value.Id}", result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarAgendamentoCommand command, CancellationToken cancellationToken)
    {
        var result = await _atualizarAgendamentoCommandHandler.Handle(command with { Id = id }, cancellationToken);
        return result.IsFailure ? HandleFailure(result.Error) : Ok(result.Value);
    }

    [HttpPatch("{id:guid}/cancelar")]
    public async Task<IActionResult> Cancelar([FromRoute] CancelarAgendamentoCommand command, CancellationToken cancellationToken)
    {
        var result = await _cancelarAgendamentoCommandHandlerHandler.Handle(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result.Error) : Ok(result.Value);
    }

    [HttpPatch("{id:guid}/concluir")]
    public async Task<IActionResult> Concluir([FromRoute] ConcluirAgendamentoCommand command, CancellationToken cancellationToken)
    {
        var result = await _concluirAgendamentoCommandHandlerHandler.Handle(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result.Error) : Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir([FromRoute] ExcluirAgendamentoCommand command, CancellationToken cancellationToken)
    {
        var result = await _excluirAgendamentoCommandHandlerHandler.Handle(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result.Error) : NoContent();
    }
}
