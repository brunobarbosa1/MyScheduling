using MyScheduling.Domain.Entities.Agendamentos;
using MyScheduling.Domain.Repositories.Agendamentos;

namespace MyScheduling.Tests.Agendamentos;

internal sealed class FakeAgendamentoRepository : IAgendamentoRepository
{
    public List<Agendamento> Salvos { get; } = [];
    public bool ExisteSobreposicao { get; set; }
    public int SaveCount { get; private set; }

    public IQueryable<Agendamento> Query() => Salvos.AsQueryable();

    public Task<bool> ExisteSobreposicaoAsync(
        DateTimeOffset dataHoraInicio,
        DateTimeOffset dataHoraFim,
        Guid? ignorarId = null,
        CancellationToken cancellationToken = default)
        => Task.FromResult(ExisteSobreposicao);

    public Task<Agendamento?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(Salvos.FirstOrDefault(a => a.Id == id));

    public void Add(Agendamento agendamento) => Salvos.Add(agendamento);

    public void Remove(Agendamento agendamento) => Salvos.Remove(agendamento);

    public Task SaveAsync(CancellationToken cancellationToken = default)
    {
        SaveCount++;
        return Task.CompletedTask;
    }
}

internal sealed class FixedTimeProvider : TimeProvider
{
    private readonly DateTimeOffset _utcNow;

    public FixedTimeProvider(DateTimeOffset utcNow) => _utcNow = utcNow;

    public override DateTimeOffset GetUtcNow() => _utcNow;
}
