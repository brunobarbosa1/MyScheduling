using Microsoft.EntityFrameworkCore;
using MyScheduling.Domain.Entities.Agendamentos;
using MyScheduling.Domain.Entities.Usuarios;

namespace MyScheduling.Data.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Agendamento> Agendamentos => Set<Agendamento>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
