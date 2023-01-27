// using System;
// using System.Collections.Generic;
// using System.Text.Json;
// using System.Threading;
// using Domain.Cacambas.Agregacao;
// using Domain.Clientes.Agrecacao;
// using Domain.Clientes.Enumeraveis;
// using Domain.Pedidos.Agregacao;
// using Domain.Pedidos.Comandos;
// using Domain.Pedidos.Enumeraveis;
// using Domain.Pedidos.Fila;
// using Domain.Pedidos.Interface;
// using Infra.Dados;
// using Infra.Pedidos.Fila;
// using Infra.Repositorios;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Diagnostics;
// using Xunit;
// using Xunit.Abstractions;

// namespace IntegrationTests.Agragacao.Comandos;

// public class Criar_NovoPedido_Teste
// {
//     const string invalidConnectionString = "DeefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
//     private readonly StorageContext _invalidStorageContext;
//     private static int nullID;
//     private readonly ITestOutputHelper helper;
//     private readonly IMensagem _mensagem;
//     private readonly IPedidoRepositorio _repositorio;
//     private readonly StorageContext _storageContext;
//     private readonly DataContext _dataContext;
//     Cliente _clienteValido;
//     Endereco _enderecoValido;
//     Pedido _pedidoValido;
//     List<PedidoItem> _itensDoPedidoValido;
//     Cacamba _cacambaValida;
//     const string connectionString = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";

//     public Criar_NovoPedido_Teste(ITestOutputHelper _helper)
//     {
//         _cacambaValida = new Cacamba("10", "3M", 100);

//         _itensDoPedidoValido = new List<PedidoItem>();
//         _itensDoPedidoValido.Add(new PedidoItem(volumeDaCacamba: "3M", precoDaAlocacao: 100));

//         _enderecoValido = new(
//             cep: "11111-111",
//             logradouro: "rua Teste",
//             numero: "150",
//             complemento: "teste",
//             bairro: "teste",
//             cidade: "teste",
//             uf: "TS"
//             );

//         _clienteValido = new(
//              nome: "Douglas Teste",
//              documento: "40230258874",
//              tipoDocumento: 0,
//              dataNascimento: DateTime.Now.AddYears(-18),
//              telefone: "11111111111",
//              nomeContato: "Douglas",
//              email: "teste@teste.com",
//              observacao: "teste",
//              contribuinte: "s",
//              endereco: _enderecoValido,
//              enderecosEntrega: new List<Endereco>() { _enderecoValido }
//              );

//         _pedidoValido = new(
//             cliente: _clienteValido,
//             enderecoDeEntregaId: _enderecoValido.Id,
//             valorTotal: 150,
//             tipoDePagamento: TipoDePagamento.Pix,
//             observacao: "",
//             pedidoItems: _itensDoPedidoValido
//         );

//         _storageContext = new StorageContext(connectionString, "pedidos");
//         _invalidStorageContext = new StorageContext(invalidConnectionString, "pedidos");
//         var options = new DbContextOptionsBuilder<DataContext>()
//         .UseInMemoryDatabase("Pedidos")
//         .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
//         .Options;

//         helper = _helper;
//         _dataContext = new DataContext(options);
//         _dataContext.Cacambas?.Add(_cacambaValida);
//         _dataContext.Clientes?.Add(_clienteValido);
//         _dataContext.Pedidos?.Add(_pedidoValido);
//         _dataContext.SaveChanges();
//         _repositorio = new PedidoRepositorio(_dataContext);
//         _mensagem = new Mensagem(_storageContext);
//     }

//     [Fact]
//     public async void Novo_Pedido_persistido_com_sucesso()
//     {
//         NovoPedidoComando comando = new(
//             ClienteId: _clienteValido.Id,
//             EnderecoId: _enderecoValido.Id,
//             TipoDePagamento: TipoDePagamento.Pix,
//             Observacao: "",
//             new() { new(volumeDaCacamba: "3M", precoDaAlocacao: 150) });

//         NovoPedidoValidacao validacao = new();
//         var validate = validacao.Validate(comando);

//         Assert.True(validate.IsValid);

//         NovoPedidoManipulador manipulador = new(_mensagem, _repositorio);
//         var result = await manipulador.Handle(comando, default(CancellationToken));
//         Assert.True(result.Sucesso);
//     }

//     [Fact]
//     public void Novo_Pedido_deve_falhar_ao_enviar_mensagem_para_fila_com_connectionString_invalida()
//     {
//         NovoPedidoComando comando = new(
//             ClienteId: _clienteValido.Id,
//             EnderecoId: _enderecoValido.Id,
//             TipoDePagamento: TipoDePagamento.Pix,
//             Observacao: "",
//             new() { new(volumeDaCacamba: "3M", precoDaAlocacao: 150) });

//         NovoPedidoValidacao validacao = new();
//         var validate = validacao.Validate(comando);

//         // Assert.Throws<FormatException>(() => new Mensagem(_invalidStorageContext));
//     }

//     [Theory]
//     [InlineData(null, 1)]
//     [InlineData(1, null)]
//     public async void Gerando_pedido_com_clienteID_ou_enderecoID_null_deve_falhar(int clienteID, int enderecoID)
//     {
//         NovoPedidoComando comando = new(
//             ClienteId: clienteID,
//             EnderecoId: enderecoID,
//             TipoDePagamento: TipoDePagamento.Pix,
//             Observacao: "",
//             new() {
//                 new(volumeDaCacamba: "3M", precoDaAlocacao: 150)
//             });

//         NovoPedidoValidacao validacao = new();
//         var validate = validacao.Validate(comando);

//         NovoPedidoManipulador manipulador = new(_mensagem, _repositorio);
//         var result = await manipulador.Handle(comando, default(CancellationToken));
//         Assert.False(result.Sucesso);
//     }

//     [Theory]
//     [InlineData(31, 1)]
//     [InlineData(1, 31)]
//     public async void Gerando_pedido_com_clienteID_ou_enderecoID_inexistente_deve_falhar(int clienteID, int enderecoID)
//     {
//         NovoPedidoComando comando = new(
//             ClienteId: clienteID,
//             EnderecoId: enderecoID,
//             TipoDePagamento: TipoDePagamento.Pix,
//             Observacao: "",
//             new() {
//                 new(volumeDaCacamba: "3M", precoDaAlocacao: 150)
//             });

//         NovoPedidoValidacao validacao = new();
//         var validate = validacao.Validate(comando);

//         NovoPedidoManipulador manipulador = new(_mensagem, _repositorio);
//         var result = await manipulador.Handle(comando, default(CancellationToken));
//         Assert.False(result.Sucesso);
//     }

//     [Fact]
//     public async void Vinculando_cacamba_com_PedidoID_nulo_deve_falhar()
//     {
//         VincularCacambaComando comando = new(
//             pedidoId: nullID,
//             pedidoItemId: 1,
//             cacambaId: 1
//         );
//         VincularCacambaValidacao validacao = new();
//         var validate = validacao.Validate(comando);

//         Assert.True(validate.IsValid);
//         VincularCacambaManipulador manipulador = new(_repositorio);
//         var result = await manipulador.Handle(comando, default(CancellationToken));
//         Assert.False(result.Sucesso);
//     }

//     [Fact]
//     public async void Vinculando_cacamba_com_PedidoItemID_nulo_deve_falhar()
//     {
//         var pedidoComItens = _repositorio.ObterPedidoComItens(_pedidoValido.Id);
//         var cacamba = _repositorio.ObterCacambaPorId(_cacambaValida.Id);

//         VincularCacambaComando comando = new(
//             pedidoId: pedidoComItens.Id,
//             pedidoItemId: nullID,
//             cacambaId: cacamba.Id);
//         VincularCacambaValidacao validacao = new();
//         var validate = validacao.Validate(comando);

//         Assert.True(validate.IsValid);
//         VincularCacambaManipulador manipulador = new(_repositorio);
//         var result = await manipulador.Handle(comando, default(CancellationToken));
//         Assert.False(result.Sucesso);
//     }

//     [Fact]
//     public async void Vinculando_cacamba_com_CacambaID_nulo_deve_falhar()
//     {
//         var pedidoComItens = _repositorio.ObterPedidoComItens(_pedidoValido.Id);
//         var item = pedidoComItens.PedidoItems?[0];

//         VincularCacambaComando comando = new(
//             pedidoId: pedidoComItens.Id,
//             pedidoItemId: item.Id,
//             cacambaId: nullID);
//         VincularCacambaValidacao validacao = new();
//         var validate = validacao.Validate(comando);

//         Assert.True(validate.IsValid);
//         VincularCacambaManipulador manipulador = new(_repositorio);
//         var result = await manipulador.Handle(comando, default(CancellationToken));
//         Assert.False(result.Sucesso);
//     }

//     [Theory]
//     [InlineData(null, 1, 1)]
//     [InlineData(1, null, 1)]
//     [InlineData(1, 1, null)]
//     public async void Vinculando_cacamba_com_pedidoID_ou_itemID_ou_cacambaID_inexistente_deve_falhar(int pedidoID, int itemID, int cacambaID)
//     {
//         VincularCacambaComando comando = new(
//             pedidoId: pedidoID,
//             pedidoItemId: itemID,
//             cacambaId: cacambaID);
//         VincularCacambaValidacao validacao = new();
//         var validate = validacao.Validate(comando);

//         Assert.True(validate.IsValid);
//         VincularCacambaManipulador manipulador = new(_repositorio);
//         var result = await manipulador.Handle(comando, default(CancellationToken));
//         Assert.False(result.Sucesso);
//     }

//     [Fact]
//     public async void Apos_vincular_uma_cacamba_seu_Status_deve_ser_Alocado()
//     {
//         var pedidoComItens = _repositorio.ObterPedidoComItens(_pedidoValido.Id);
//         var item = pedidoComItens.PedidoItems?[0];
//         var cacamba = _repositorio.ObterCacambaPorId(_cacambaValida.Id);

//         VincularCacambaComando comando = new(
//             pedidoId: pedidoComItens.Id,
//             pedidoItemId: item.Id,
//             cacambaId: cacamba.Id);

//         VincularCacambaValidacao validacao = new();
//         var validate = validacao.Validate(comando);
//         Assert.True(validate.IsValid);

//         VincularCacambaManipulador manipulador = new(_repositorio);
//         var result = await manipulador.Handle(comando, default(CancellationToken));

//         Assert.Equal(Domain.Cacambas.Enumeraveis.Status.Alocado, cacamba.Status);
//     }
// }