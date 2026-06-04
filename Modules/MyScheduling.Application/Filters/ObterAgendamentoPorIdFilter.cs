namespace MyScheduling.Application.Agendamentos.Queries;

public sealed record ObterAgendamentoPorIdFilter
{
    public Guid Id { get; init; }
}
