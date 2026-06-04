using MyScheduling.Domain.Entities;

namespace MyScheduling.Domain.Repositories;

public interface IAgendamentoRepository
{
    IQueryable<Agendamento> Query();

    Task<bool> ExisteSobreposicaoAsync(
        DateTimeOffset dataHoraInicio,
        DateTimeOffset dataHoraFim,
        Guid? ignorarId = null,
        CancellationToken cancellationToken = default);

    Task<Agendamento?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Agendamento agendamento);
    void Remove(Agendamento agendamento);
    Task SaveAsync(CancellationToken cancellationToken = default);
}
