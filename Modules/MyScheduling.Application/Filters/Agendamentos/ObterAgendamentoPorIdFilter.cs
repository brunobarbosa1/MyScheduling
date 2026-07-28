namespace MyScheduling.Application.Filters.Agendamentos;

public sealed record ObterAgendamentoPorIdFilter
{
    public Guid Id { get; init; }
}
