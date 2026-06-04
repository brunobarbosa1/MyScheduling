using Microsoft.EntityFrameworkCore;
using MyScheduling.Common.Results;
using MyScheduling.Domain.Repositories;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.Agendamentos.Queries;

public sealed class ListarAgendamentosPorDataQuery : IListarAgendamentosPorDataQuery
{
    private readonly IAgendamentoRepository _agendamentoRepository;

    public ListarAgendamentosPorDataQuery(IAgendamentoRepository agendamentoRepository)
    {
        _agendamentoRepository = agendamentoRepository;
    }

    public async Task<Result<IReadOnlyList<AgendamentoViewModel>>> Handle(
        ListarAgendamentosPorDataFilter filter,
        CancellationToken cancellationToken = default)
    {
        var inicioDia = new DateTimeOffset(filter.Data.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero);
        var fimDia = inicioDia.AddDays(1);

        var agendamentos = await _agendamentoRepository.Query()
            .AsNoTracking()
            .Where(a => a.DataHoraInicio >= inicioDia && a.DataHoraInicio < fimDia)
            .OrderBy(a => a.DataHoraInicio)
            .Select(AgendamentoProjections.ToViewModel)
            .ToListAsync(cancellationToken);

        return Result.Success<IReadOnlyList<AgendamentoViewModel>>(agendamentos);
    }
}
