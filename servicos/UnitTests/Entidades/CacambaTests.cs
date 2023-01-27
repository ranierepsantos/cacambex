using Domain.Cacambas.Agregacao;
using Xunit;


namespace UnitTests.Entidades;
public class CacambaTests
{
    public CacambaTests()
    {

    }
    [Fact]
    public void AoPassarDadosNecessarios_CriarCacamba_ComSucesso()
    {
        Cacamba cacamba = new("1010", "10M", 9M);
        Assert.NotEmpty(cacamba.Numero);
        Assert.NotNull(cacamba.Preco);
    }

}