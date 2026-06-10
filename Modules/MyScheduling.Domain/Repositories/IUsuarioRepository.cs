using MyScheduling.Domain.Entities;

namespace MyScheduling.Domain.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    void Add(Usuario usuario);
    Task SaveAsync(CancellationToken cancellationToken = default);
}
