namespace MyScheduling.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public DateTime? AtualizadoEm { get; private set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
        CriadoEm = DateTime.UtcNow;
    }

    protected void MarcarComoAtualizado()
    {
        AtualizadoEm = DateTime.UtcNow;
    }
}