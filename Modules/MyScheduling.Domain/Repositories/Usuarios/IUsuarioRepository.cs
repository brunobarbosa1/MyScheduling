using MyScheduling.Domain.Entities.Usuarios;

namespace MyScheduling.Domain.Repositories.Usuarios;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    void Add(Usuario usuario);
    Task SaveAsync(CancellationToken cancellationToken = default);
}
