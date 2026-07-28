using Microsoft.EntityFrameworkCore;
using MyScheduling.Data.Contexts;
using MyScheduling.Domain.Entities.Usuarios;
using MyScheduling.Domain.Repositories.Usuarios;

namespace MyScheduling.Data.Repositories.Usuarios;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public void Add(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
    }

    public Task SaveAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
