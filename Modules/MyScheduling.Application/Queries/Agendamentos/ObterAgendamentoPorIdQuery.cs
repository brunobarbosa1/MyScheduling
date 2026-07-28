using Microsoft.EntityFrameworkCore;
using MyScheduling.Application.Filters.Agendamentos;
using MyScheduling.Common.Results;
using MyScheduling.Domain.Repositories.Agendamentos;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.Queries.Agendamentos;

public sealed class ObterAgendamentoPorIdQuery : IObterAgendamentoPorIdQuery
{
    private readonly IAgendamentoRepository _agendamentoRepository;

    public ObterAgendamentoPorIdQuery(IAgendamentoRepository agendamentoRepository)
    {
        _agendamentoRepository = agendamentoRepository;
    }

    public async Task<Result<AgendamentoViewModel>> Handle(
        ObterAgendamentoPorIdFilter filter,
        CancellationToken cancellationToken = default)
    {
        var agendamento = await _agendamentoRepository.Query()
            .AsNoTracking()
            .Where(a => a.Id == filter.Id)
            .Select(AgendamentoProjections.ToViewModel)
            .FirstOrDefaultAsync(cancellationToken);

        if (agendamento is null)
            return Result.Failure<AgendamentoViewModel>(
                Error.NotFound("agendamento.naoEncontrado", "Agendamento não encontrado."));

        return Result.Success(agendamento);
    }
}
