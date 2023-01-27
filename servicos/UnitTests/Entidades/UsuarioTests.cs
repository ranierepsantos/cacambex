using Domain.Identidade.Agregacao;
using Domain.Identidade.Enumeraveis;
using Xunit;

namespace UnitTests.Entidades;
public class UsuarioTests
{
    public UsuarioTests()
    {

    }
    [Fact]
    public void PassandoDadosCorretos_CriarCliente_ComSucesso()
    {
        Usuario usuario = new("Theo", "theo@theo.com", Funcao.Cliente);
        Assert.NotEmpty(usuario.Email);
        Assert.NotNull(usuario.Email);
    }
}