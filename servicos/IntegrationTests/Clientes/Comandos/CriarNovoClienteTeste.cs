using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Clientes.Comando;
public class CriarNovoClienteTeste
{


    public CriarNovoClienteTeste(ITestOutputHelper helper)
    {
    }

    [Fact]
    public void AoCriarNovoClienteNaOmie_ComDadosValidos_RetornarSucesso()
    {

    }

    [Fact]
    public void AoCriarNovoCliente_ComUmOuMaisCamposVazio_RetornarFalha()
    {

    }
    [Fact]
    public void AoCriarNovoCliente_ComListaDeEnderecoVazia_RetornarFalha()
    {
    }
    [Fact]
    public void AoAtualizarCliente_ComDadosValidos_RetornarSucesso()
    {

    }
}