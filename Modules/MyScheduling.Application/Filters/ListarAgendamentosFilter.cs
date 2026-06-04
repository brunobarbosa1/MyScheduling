using MyScheduling.Domain.Enums;

namespace MyScheduling.Application.Filters;

public sealed record ListarAgendamentosFilter
{
    /// <summary>Filtro opcional por status. Quando nulo, retorna todos.</summary>
    public StatusAgendamento? Status { get; init; }
}
