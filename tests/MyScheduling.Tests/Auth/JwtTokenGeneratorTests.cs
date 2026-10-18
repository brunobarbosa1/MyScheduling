using System.IdentityModel.Tokens.Jwt;
using MyScheduling.Domain.Entities;
using MyScheduling.Service.Security;
using MyScheduling.Tests.Agendamentos;
using Xunit;

namespace MyScheduling.Tests.Auth;

public class JwtTokenGeneratorTests
{
    private static readonly DateTimeOffset Agora = new(2026, 6, 10, 12, 0, 0, TimeSpan.Zero);

    private static readonly JwtOptions Options = new()
    {
        Issuer = "myscheduling",
        Audience = "myscheduling-api",
        SecretKey = "chave-super-secreta-com-mais-de-256-bits-1234567890",
        ExpirationSeconds = 3600
    };

    [Fact]
    public void Generate_RetornaTokenComExpiracaoConfigurada()
    {
        var generator = new JwtTokenGenerator(Options, new FixedTimeProvider(Agora));
        var usuario = new Usuario("Admin", "admin@salao.com", "hash");

        var result = generator.Generate(usuario);

        Assert.NotEmpty(result.AccessToken);
        Assert.Equal(3600, result.ExpiresInSeconds);
    }

    [Fact]
    public void Generate_TokenContemClaimsDoUsuarioEExpiracao()
    {
        var generator = new JwtTokenGenerator(Options, new FixedTimeProvider(Agora));
        var usuario = new Usuario("Admin", "admin@salao.com", "hash");

        var result = generator.Generate(usuario);

        var token = new JwtSecurityTokenHandler().ReadJwtToken(result.AccessToken);

        Assert.Equal(Options.Issuer, token.Issuer);
        Assert.Contains(Options.Audience, token.Audiences);
        Assert.Equal(usuario.Id.ToString(), token.Subject);
        Assert.Contains(token.Claims, c => c.Type == "email" && c.Value == usuario.Email);
        Assert.Equal(Agora.AddSeconds(3600).UtcDateTime, token.ValidTo);
    }
}
