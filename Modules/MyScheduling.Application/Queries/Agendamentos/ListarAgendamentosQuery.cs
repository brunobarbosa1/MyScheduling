using Microsoft.EntityFrameworkCore;
using MyScheduling.Application.Filters.Agendamentos;
using MyScheduling.Common.Results;
using MyScheduling.Domain.Repositories.Agendamentos;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.Queries.Agendamentos;

public sealed class ListarAgendamentosQuery : IListarAgendamentosQuery
{
    private readonly IAgendamentoRepository _agendamentoRepository;

    public ListarAgendamentosQuery(IAgendamentoRepository agendamentoRepository)
    {
        _agendamentoRepository = agendamentoRepository;
    }

    public async Task<Result<IReadOnlyList<AgendamentoViewModel>>> Handle(
        ListarAgendamentosFilter filter,
        CancellationToken cancellationToken = default)
    {
        var consulta = _agendamentoRepository.Query().AsNoTracking();

        if (filter.Status is not null)
            consulta = consulta.Where(a => a.Status == filter.Status);

        var agendamentos = await consulta
            .OrderBy(a => a.DataHoraInicio)
            .Select(AgendamentoProjections.ToViewModel)
            .ToListAsync(cancellationToken);

        return Result.Success<IReadOnlyList<AgendamentoViewModel>>(agendamentos);
    }
}
