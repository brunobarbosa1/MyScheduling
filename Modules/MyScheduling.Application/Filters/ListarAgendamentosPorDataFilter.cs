namespace MyScheduling.Application.Agendamentos.Queries;

public sealed record ListarAgendamentosPorDataFilter
{
    public DateOnly Data { get; init; }
}
