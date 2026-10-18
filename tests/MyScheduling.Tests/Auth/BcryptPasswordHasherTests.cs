using MyScheduling.Service.Security;
using Xunit;

namespace MyScheduling.Tests.Auth;

public class BcryptPasswordHasherTests
{
    private readonly BcryptPasswordHasher _hasher = new();

    [Fact]
    public void Hash_NaoRetornaSenhaEmTextoPuro()
    {
        var hash = _hasher.Hash("senha-secreta");

        Assert.NotEqual("senha-secreta", hash);
        Assert.NotEmpty(hash);
    }

    [Fact]
    public void Verify_SenhaCorreta_RetornaTrue()
    {
        var hash = _hasher.Hash("senha-secreta");

        Assert.True(_hasher.Verify("senha-secreta", hash));
    }

    [Fact]
    public void Verify_SenhaIncorreta_RetornaFalse()
    {
        var hash = _hasher.Hash("senha-secreta");

        Assert.False(_hasher.Verify("senha-errada", hash));
    }
}
