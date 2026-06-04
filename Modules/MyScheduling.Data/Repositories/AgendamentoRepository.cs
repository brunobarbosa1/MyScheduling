using Microsoft.EntityFrameworkCore;
using MyScheduling.Data.Contexts;
using MyScheduling.Domain.Entities;
using MyScheduling.Domain.Enums;
using MyScheduling.Domain.Repositories;

namespace MyScheduling.Data.Repositories;

public class AgendamentoRepository : IAgendamentoRepository
{
    private readonly AgendamentoContext _context;

    public AgendamentoRepository(AgendamentoContext context)
    {
        _context = context;
    }

    public IQueryable<Agendamento> Query()
    {
        return _context.Agendamentos;
    }

    public Task<bool> ExisteSobreposicaoAsync(
        DateTimeOffset dataHoraInicio,
        DateTimeOffset dataHoraFim,
        Guid? ignorarId = null,
        CancellationToken cancellationToken = default)
    {
        return _context.Agendamentos
            .AsNoTracking()
            .Where(a => a.Status != StatusAgendamento.Cancelado)
            .Where(a => ignorarId == null || a.Id != ignorarId)
            .AnyAsync(
                a => a.DataHoraInicio < dataHoraFim && dataHoraInicio < a.DataHoraFim,
                cancellationToken);
    }

    public Task<Agendamento?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Agendamentos
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public void Add(Agendamento agendamento)
    {
        _context.Agendamentos.Add(agendamento);
    }

    public void Remove(Agendamento agendamento)
    {
        _context.Agendamentos.Remove(agendamento);
    }

    public Task SaveAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
