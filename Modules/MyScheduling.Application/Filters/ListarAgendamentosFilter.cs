using MyScheduling.Domain.Enums;

namespace MyScheduling.Application.Agendamentos.Queries;

public sealed record ListarAgendamentosFilter
{
    /// <summary>Filtro opcional por status. Quando nulo, retorna todos.</summary>
    public StatusAgendamento? Status { get; init; }
}
