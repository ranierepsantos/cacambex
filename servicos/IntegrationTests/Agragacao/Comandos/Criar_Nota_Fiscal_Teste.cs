// using System;
// using System.Collections.Generic;
// using System.Text.Json;
// using System.Threading;
// using Domain.Clientes.Agrecacao;
// using Domain.Clientes.Enumeraveis;
// using Domain.Pedidos.Agregacao;
// using Domain.Pedidos.Comandos;
// using Domain.Pedidos.Enumeraveis;
// using Domain.Pedidos.Execoes;
// using Domain.Pedidos.Interface;
// using Infra.Dados;
// using Infra.Repositorios;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Diagnostics;
// using Xunit;
// using Xunit.Abstractions;

// namespace IntegrationTests.Agragacao.Comandos;
// public class Criar_Nota_Fiscal_Teste
// {
//     private readonly IPedidoRepositorio _repositorio;
//     private readonly DataContext _context;
//     Pedido _pedidoValido;
//     Cliente _clienteValido;
//     List<PedidoItem> _itensDoPedidoValido;
//     Endereco _enderecoValido;

//     public Criar_Nota_Fiscal_Teste()
//     {
//         _itensDoPedidoValido = new List<PedidoItem>();
//         _itensDoPedidoValido.Add(new PedidoItem(volumeDaCacamba: "3M", precoDaAlocacao: 150));

//         _enderecoValido = new(
//            cep: "11111-111",
//            logradouro: "rua Teste",
//            numero: "150",
//            complemento: "teste",
//            bairro: "teste",
//            cidade: "teste",
//            uf: "TS"
//            );

//         _clienteValido = new(
//             nome: "Douglas Teste",
//             documento: "40230258874",
//             tipoDocumento: 0,
//             dataNascimento: DateTime.Now.AddYears(-18),
//             telefone: "11111111111",
//             nomeContato: "Douglas",
//             email: "teste@teste.com",
//             observacao: "teste",
//             contribuinte: "s",
//             endereco: _enderecoValido,
//             enderecosEntrega: new List<Endereco>() { _enderecoValido }
//             );

//         _pedidoValido = new(
//             cliente: _clienteValido,
//             enderecoDeEntregaId: _enderecoValido.Id,
//             valorTotal: 150,
//             tipoDePagamento: TipoDePagamento.Pix,
//             observacao: "",
//             pedidoItems: _itensDoPedidoValido
//             );

//         var options = new DbContextOptionsBuilder<DataContext>()
//        .UseInMemoryDatabase("Pedidos")
//        .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
//        .Options;
//         _context = new DataContext(options);
//         _context.Clientes?.Add(_clienteValido);
//         _context.Pedidos?.Add(_pedidoValido);
//         _context.SaveChanges();
//         _repositorio = new PedidoRepositorio(_context);

//     }

//     [Fact]
//     public async void Nota_Fiscal_emitida_com_sucesso()
//     {
//         var pedidoComItens = _repositorio.ObterPedidoPorId(_pedidoValido.Id);
//         NovaNotaFiscalComando comando = new(PedidoId: pedidoComItens.Id);

//         NovaNotaFiscalValidacao validacao = new();
//         var validate = validacao.Validate(comando);

//         CriarNotaFiscalManipulador manilupador = new(_repositorio);
//         var result = await manilupador.Handle(comando, default(CancellationToken));
//         Assert.True(result.Sucesso);
//     }

//     [Fact]
//     public async void AoEmitirUmaNotaFiscal_ComSucesso_SetarSeuStatusComoConcluido()
//     {
//         var pedidoComItens = _repositorio.ObterPedidoPorId(_pedidoValido.Id);
//         NovaNotaFiscalComando comando = new(PedidoId: pedidoComItens.Id);

//         NovaNotaFiscalValidacao validacao = new();
//         var validate = validacao.Validate(comando);

//         CriarNotaFiscalManipulador manilupador = new(_repositorio);
//         var result = await manilupador.Handle(comando, default(CancellationToken));
//         Assert.Equal(StatusPedido.Concluido, pedidoComItens.NotaFiscal?.Status);
//     }

//     [Fact]
//     public async void AoEmitirUmaNotaFiscal_ComPedidoIdErpZeroOuNulo_DeveFalhar()
//     {
//         var pedidoComItens = _repositorio.ObterPedidoPorId(_pedidoValido.Id);

//         NovaNotaFiscalComando comando = new(pedidoComItens.Id);
//         NovaNotaFiscalValidacao validacao = new();
//         var validate = validacao.Validate(comando);

//         CriarNotaFiscalManipulador manilupador = new(_repositorio);
//         var result = await manilupador.Handle(comando, default(CancellationToken));
//         Assert.False(result.Sucesso);
//     }

//     [Fact]
//     public async void AoEmitirUmaNotaFiscal_ComPedidoIdErpZeroOuNulo_RetornarStatusNotaFiscalComFalha()
//     {
//         var pedidoComItens = _repositorio.ObterPedidoPorId(_pedidoValido.Id);

//         NovaNotaFiscalComando comando = new(pedidoComItens.Id);
//         NovaNotaFiscalValidacao validacao = new();
//         var validate = validacao.Validate(comando);

//         CriarNotaFiscalManipulador manilupador = new(_repositorio);
//         await manilupador.Handle(comando, default(CancellationToken));
//         Assert.Equal(StatusPedido.ComFalhas, pedidoComItens.NotaFiscal?.Status);
//     }

// }