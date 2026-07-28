namespace MyScheduling.Domain.Entities.Usuarios;

public class Usuario : Entity
{
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }

    private Usuario()
    {
        Nome = null!;
        Email = null!;
        PasswordHash = null!;
    }

    public Usuario(string nome, string email, string passwordHash)
    {
        Nome = nome;
        Email = email;
        PasswordHash = passwordHash;
    }
}
