namespace MyScheduling.Application.Filters;

public sealed record ListarAgendamentosPorDataFilter
{
    public DateOnly Data { get; init; }
}
