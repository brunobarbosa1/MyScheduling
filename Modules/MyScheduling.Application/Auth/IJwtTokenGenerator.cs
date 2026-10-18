using MyScheduling.Domain.Entities;

namespace MyScheduling.Application.Auth;

public interface IJwtTokenGenerator
{
    GeneratedToken Generate(Usuario usuario);
}

public sealed record GeneratedToken(string AccessToken, int ExpiresInSeconds);
