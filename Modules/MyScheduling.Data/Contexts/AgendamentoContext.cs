using Microsoft.EntityFrameworkCore;
using MyScheduling.Domain.Entities;

namespace MyScheduling.Data.Contexts;

public class AgendamentoContext : DbContext
{
    public AgendamentoContext(DbContextOptions<AgendamentoContext> options) : base(options)
    {
    }

    public DbSet<Agendamento> Agendamentos => Set<Agendamento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AgendamentoContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
