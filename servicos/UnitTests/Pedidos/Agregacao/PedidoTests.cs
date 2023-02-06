using System;
using System.Collections.Generic;
using Domain.Cacambas.Agregacao;
using Domain.Clientes.Agrecacao;
using Domain.Clientes.Enumeraveis;
using Domain.Pedidos.Agregacao;
using Domain.Pedidos.Enumeraveis;
using Xunit;

namespace UnitTests.Pedidos.Agregacao;

public class PedidoTests
{

    private readonly Cliente _clienteValido;
    private readonly EnderecoCobranca _enderecoCobrancaValido;
    private readonly List<EnderecoEntrega> _enderecoEntregaValido;
    private readonly PedidoItem _pedidoItem;
    public PedidoTests()
    {

        _enderecoCobrancaValido = new EnderecoCobranca(
            cep: "13219110",
            logradouro: "rua teste",
            numero: "1",
            complemento: "casa verde",
            bairro: "teste",
            cidade: "Jundiaí",
            uf: "sp");

        _enderecoEntregaValido = new List<EnderecoEntrega>(){
            new EnderecoEntrega(
            cep: "13219110",
            logradouro: "rua teste",
            numero: "1",
            complemento: "casa verde",
            bairro: "teste",
            cidade: "Jundiaí",
            uf: "sp")
        };

        _clienteValido = new Cliente(
             nome: "Douglas",
            documento: "10120230345",
            tipoDocumento: TipoDocumento.Cpf,
            dataNascimento: DateTime.Now.AddYears(-18),
            telefone: "952526363",
            email: "douglas2@douglas2.com",
            contribuinte: "s",
            enderecoCobranca: _enderecoCobrancaValido,
            enderecosEntrega: _enderecoEntregaValido);

        _pedidoItem = new("3M", 100);
    }
    [Fact]
    public void Pedido_construido_com_sucesso()
    {
        long nCodOs = 1;
        Guid codigoInternoOS = Guid.NewGuid();
        var pedido = new Pedido(
            cliente: _clienteValido,
            pedidoItem: _pedidoItem,
            enderecoEntrega: _enderecoEntregaValido[0],
            tipoDePagamento: TipoDePagamento.Boleto,
            observacao: "",
            valorPedido: 100,
            codigoInternoOS,
            nCodOs);

        Assert.Equal(100, pedido.ValorPedido);
    }

    [Fact]
    public void Vincular_Cacamba_com_Sucesso()
    {
        Guid codigoInternoOS = Guid.NewGuid();
        long nCodOs = 1;
        var pedido = new Pedido(
                    cliente: _clienteValido,
                    pedidoItem: _pedidoItem,
                    enderecoEntrega: _enderecoEntregaValido[0],
                    tipoDePagamento: TipoDePagamento.Boleto,
                    observacao: "",
                    valorPedido: 100,
                    codigoInternoOS,
                    nCodOs);

        Cacamba cacamba = new("130311", "5M", 1);
        pedido.PedidoItem.VincularCacamba(cacamba);

        Assert.True(true);
    }
}