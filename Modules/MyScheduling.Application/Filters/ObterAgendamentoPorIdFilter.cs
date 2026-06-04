namespace MyScheduling.Application.Filters;

public sealed record ObterAgendamentoPorIdFilter
{
    public Guid Id { get; init; }
}
