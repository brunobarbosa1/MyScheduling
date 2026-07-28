using MyScheduling.Domain.Enums.Agendamentos;

namespace MyScheduling.Application.Filters.Agendamentos;

public sealed record ListarAgendamentosFilter
{
    /// <summary>Filtro opcional por status. Quando nulo, retorna todos.</summary>
    public StatusAgendamento? Status { get; init; }
}
