namespace MyScheduling.Application.Filters.Agendamentos;

public sealed record ListarAgendamentosPorDataFilter
{
    public DateOnly Data { get; init; }
}
