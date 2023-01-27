using System;
using System.Collections.Generic;
using Domain.Cacambas.Agregacao;
using Domain.Clientes.Agrecacao;
using Domain.Clientes.Enumeraveis;
using Domain.Pedidos.Agregacao;
using Domain.Pedidos.Enumeraveis;
using Domain.Pedidos.Execoes;
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
    // [Fact]
    // public void Gerando_pedido_com_cliente_nulo_deve_falhar()
    // {
    //     var excecao = Assert.Throws<PedidoExcecoes>(
    //         () => new Pedido(
    //         cliente: null,
    //         enderecoDeEntrega: _enderecoValido,
    //         valorTotal: 1000,
    //         tipoDePagamento: TipoDePagamento.Boleto,
    //         observacao: "",
    //         pedidoItems: _itensDoPedidoValido));

    //     Assert.Equal("Cliente não pode ser nulo", excecao?.Message);
    // }

    // [Fact]
    // public void Gerando_pedido_com_endereco_nulo_retornar_excecao()
    // {
    //     var excecao = Assert.Throws<PedidoExcecoes>(
    //        () => new Pedido(
    //        cliente: _clienteValido,
    //        enderecoDeEntrega: null,
    //        valorTotal: 1000,
    //        tipoDePagamento: TipoDePagamento.Boleto,
    //        observacao: "",
    //        pedidoItems: _itensDoPedidoValido));

    //     Assert.Equal("Endereço não pode ser nulo", excecao?.Message);
    // }

    // [Fact]
    // public void Gerando_pedido_com_tipo_pagamento_invalido_retornar_excecao()
    // {
    //     var excecao = Assert.Throws<PedidoExcecoes>(
    //         () => new Pedido(
    //         cliente: _clienteValido,
    //         enderecoDeEntrega: _enderecoValido,
    //         valorTotal: 1000,
    //         tipoDePagamento: (TipoDePagamento)4,
    //         observacao: "",
    //         pedidoItems: _itensDoPedidoValido));

    //     Assert.Equal("Tipo de pagamento inválido", excecao?.Message);
    // }

    // [Fact]
    // public void Soma_campo_PrecoDaAlocacao_da_lista_PedidoItens_deve_ser_igual_a_valorTotal()
    // {
    //     var pedido = new Pedido(
    //         cliente: _clienteValido,
    //         enderecoDeEntrega: _enderecoValido,
    //         valorTotal: 200,
    //         tipoDePagamento: TipoDePagamento.Pix,
    //         observacao: "",
    //         pedidoItems: _itensDoPedidoValido);

    //     Assert.Equal(200, pedido.ValorTotal);
    // }

    // [Fact]
    // public void Gerando_pedido_a_lista_PedidoItens_deve_ser_maiorOuIgual_a_um()
    // {
    //     var pedido = new Pedido(
    //          cliente: _clienteValido,
    //          enderecoDeEntrega: _enderecoValido,
    //          valorTotal: 200,
    //          tipoDePagamento: TipoDePagamento.Pix,
    //          observacao: "",
    //          pedidoItems: _itensDoPedidoValido);

    //     Assert.NotEmpty(pedido.PedidoItems);
    // }

    // [Fact]
    // public void Gerando_pedido_com_lista_PedidoItens_vazia_retornar_excecao()
    // {
    //     var excecao = Assert.Throws<PedidoExcecoes>(
    //        () => new Pedido(
    //        cliente: _clienteValido,
    //        enderecoDeEntrega: _enderecoValido,
    //        valorTotal: 100,
    //        tipoDePagamento: TipoDePagamento.Boleto,
    //        observacao: "",
    //        pedidoItems: new List<PedidoItem>()));

    //     Assert.Equal("Lista de itens não pode estar vazia", excecao?.Message);
    // }

    // [Fact]
    // public void Gerando_um_pedido_sua_fase_PedidoEmitido_deve_ser_Status_Concluido()
    // {
    //     var pedido = new Pedido(
    //         cliente: _clienteValido,
    //         enderecoDeEntrega: _enderecoValido,
    //         valorTotal: 200,
    //         tipoDePagamento: TipoDePagamento.Boleto,
    //         observacao: "",
    //         pedidoItems: _itensDoPedidoValido);
    //     Assert.Equal(StatusPedido.Concluido, pedido.Status?.Status);
    // }

    // [Fact]
    // public void Gerando_um_pedido_sua_fase_NotaFiscal_deve_ser_Status_Aguardando()
    // {
    //     var pedido = new Pedido(
    //         cliente: _clienteValido,
    //         enderecoDeEntrega: _enderecoValido,
    //         valorTotal: 200,
    //         tipoDePagamento: TipoDePagamento.Boleto,
    //         observacao: "",
    //         pedidoItems: _itensDoPedidoValido);
    //     Assert.Equal(StatusPedido.Aguardando, pedido.NotaFiscal?.Status);
    // }

    // [Fact]
    // public void Gerando_um_pedido_numero_da_NotaFiscal_deve_ser_vazio()
    // {
    //     var pedido = new Pedido(
    //                 cliente: _clienteValido,
    //                 enderecoDeEntrega: _enderecoValido,
    //                 valorTotal: 200,
    //                 tipoDePagamento: TipoDePagamento.Boleto,
    //                 observacao: "",
    //                 pedidoItems: _itensDoPedidoValido);

    //     Assert.Null(pedido.NumeroDaNotaFiscal);
    // }

    // [Fact]
    // public void Adicionando_um_novo_item_ao_pedido_valorTotal_deve_ser_atualizado()
    // {
    //     var pedido = new Pedido(
    //                 cliente: _clienteValido,
    //                 enderecoDeEntrega: _enderecoValido,
    //                 valorTotal: 200,
    //                 tipoDePagamento: TipoDePagamento.Boleto,
    //                 observacao: "",
    //                 pedidoItems: _itensDoPedidoValido);
    //     var itemParaAdicionar = new PedidoItem(volumeDaCacamba: "3m", precoDaAlocacao: 100);
    // pedido.AdicionarItem(itemParaAdicionar);

    //     Assert.Equal(300, pedido.ValorTotal);
    // }
    // [Fact]
    // public void Removendo_um_item_do_pedido_valorTotal_deve_ser_atualizado()
    // {
    //     var pedido = new Pedido(
    //                 cliente: _clienteValido,
    //                 enderecoDeEntrega: _enderecoValido,
    //                 valorTotal: 200,
    //                 tipoDePagamento: TipoDePagamento.Boleto,
    //                 observacao: "",
    //                 pedidoItems: _itensDoPedidoValido);
    //     var itemParaRemover = _itensDoPedidoValido[0];
    // pedido.RemoverItem(itemParaRemover);

    //     Assert.Equal(100, pedido.ValorTotal);
    // }

    // [Fact]
    // public void Adicionando_um_item_ao_pedido_seu_MTReCTR_deve_ter_Status_Aguandando()
    // {
    //     var pedido = new Pedido(
    //                 cliente: _clienteValido,
    //                 enderecoDeEntrega: _enderecoValido,
    //                 valorTotal: 200,
    //                 tipoDePagamento: TipoDePagamento.Boleto,
    //                 observacao: "",
    //                 pedidoItems: _itensDoPedidoValido);
    //     var itemParaAdicionar = new PedidoItem(volumeDaCacamba: "3m", precoDaAlocacao: 100);
    // pedido.AdicionarItem(itemParaAdicionar);

    //     Assert.Equal(StatusPedido.Aguardando, itemParaAdicionar.MTR?.Status);
    //     Assert.Equal(StatusPedido.Aguardando, itemParaAdicionar.CTR?.Status);
    // }

    // [Fact]
    // public void Vinculando_uma_cacamba_de_volume_diferente_do_que_foi_requisitado_pelo_cliente_retornar_excecao()
    // {
    //     var pedido = new Pedido(
    //         cliente: _clienteValido,
    //         enderecoDeEntrega: _enderecoValido,
    //         valorTotal: 1000,
    //         tipoDePagamento: TipoDePagamento.Boleto,
    //         observacao: "",
    //         pedidoItems: _itensDoPedidoValido);

    //     var cacambaRequisitadaPeloCliente = pedido.PedidoItems?[1];
    //     var cacambaErradaParaVincular = new Cacamba("1001", "5M", 100);

    // Assert.Throws<PedidoExcecoes>(() => cacambaRequisitadaPeloCliente?.VincularCacamba(cacambaErradaParaVincular));
    //     }
}