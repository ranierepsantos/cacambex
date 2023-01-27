using System;
using Domain.NotasFiscais;
using Xunit;

namespace UnitTests.Pedidos.Agregacao;
public class NotaFiscalTests
{
    [Fact]
    public void Emitir_NotaFiscal_com_sucesso()
    {
        var notaFiscal = new NotaFiscal(
            cCodIntOs: "1",
            nCodOS: 1234
        );

        Assert.NotNull(notaFiscal);
    }

    [Theory]
    [InlineData("1", null)]
    [InlineData("", 1234)]
    public void Emitir_NotaFiscal_com_um_ou_mais_campos_vazio_deve_falhar(string codigoInternoOS, int codigoOS)
    {
        var excecao = Assert.Throws<Exception>(() => new NotaFiscal(
             cCodIntOs: codigoInternoOS,
             nCodOS: codigoOS));
    }

    [Fact]
    public void Quando_uma_NotaFiscal_for_emitida_seu_status_deve_ser_concluido()
    {
        var notaFiscal = new NotaFiscal(
            cCodIntOs: "1",
            nCodOS: 1234
        );

    }
}