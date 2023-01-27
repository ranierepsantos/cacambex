using Domain.Cacambas.Interface;
using Domain.Clientes.Interface;
using Domain.Compartilhado;
using Domain.Omie;
using Domain.Omie.Pedidos.OmiePedidoRequests;
using Domain.Pedidos.Agregacao;
using Domain.Pedidos.Enumeraveis;
using Domain.Pedidos.Fila;
using Domain.Pedidos.Interface;
using Domain.Pedidos.Validacoes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Pedidos.Comandos;

public record NovoPedidoComando(
        int ClienteId,
        string VolumeCacamba,
        int EnderecoId,
        TipoDePagamento TipoDePagamento,
        string Observacao) : IRequest<Resposta>;

public class NovoPedidoManipulador : IRequestHandler<NovoPedidoComando, Resposta>
{
    private readonly IPedidoRepositorio _pedidoRepositorio;
    private readonly IClienteRepositorio _clienteRepositorio;
    private readonly ICacambaRepositorio _cacambaRepositorio;
    private readonly OmieInformacoesAdicionais _informacoesAdicionais;
    private readonly ILogger<NovoPedidoManipulador> _logger;
    private readonly IMediator _mediator;
    private readonly IMensagem _mensagem;

    public NovoPedidoManipulador(IMensagem mensagem,
                                 IPedidoRepositorio pedidoRepositorio,
                                 IClienteRepositorio clienteRepositorio,
                                 ICacambaRepositorio cacambaRepositorio,
                                 ILogger<NovoPedidoManipulador> logger,
                                 OmieInformacoesAdicionais informacoesAdicionais,
                                 IMediator mediator)
    {
        _mensagem = mensagem;
        _pedidoRepositorio = pedidoRepositorio;
        _clienteRepositorio = clienteRepositorio;
        _cacambaRepositorio = cacambaRepositorio;
        _logger = logger;
        _informacoesAdicionais = informacoesAdicionais;
        _mediator = mediator;
    }

    public async Task<Resposta> Handle(NovoPedidoComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("**********Processo de criacao de pedido iniciado.**********");

        #region validacoes
        if (request is null)
        {
            _logger.LogError("**********Request nao pode ser vazio.**********");
            return new("Request nao pode ser vazio.", false);
        }

        var validacao = new NovoPedidoValidacao();
        var resultadoValidacao = validacao.Validate(request);
        if (!resultadoValidacao.IsValid)
        {
            _logger.LogError(@"
        **********Processo de criacao de pedido falhou devido a erros de validacao**********");
            return new("", false, resultadoValidacao.Errors.Select(x => x.ErrorMessage));

        }

        var cliente = _clienteRepositorio.ObterClientePorIdComEndereco(request.ClienteId);
        if (cliente is null)
        {
            _logger.LogError("**********Cliente nao encontrado.**********");
            return new("Cliente nao encontrado.", false);
        }

        var cacamba = _cacambaRepositorio.ObterPorVolume(request.VolumeCacamba);
        if (cacamba is null)
        {
            _logger.LogError("**********Nao existem cacambas com esse volume.**********");
            return new("Nao existem cacambas com esse volume.", false);
        }

        var enderecoEntrega = _clienteRepositorio.ObterEnderecoEntregaDoCliente(request.EnderecoId);
        if (enderecoEntrega is null)
        {
            _logger.LogError("**********Endereco de entrega nao encontrado.**********");
            return new("Endereco de entrega nao encontrado.", false);
        }
        #endregion

        #region omieRequest

        Guid codigoIntegracaoOmie = Guid.NewGuid();
        int nQtdeParc = 1;
        string cEtapa = "10";
        Cabecalho cabecalho = new(codigoIntegracaoOmie, cliente.Codigo_cliente_omie, nQtdeParc, cEtapa);

        var cidadeCliente = cliente.EnderecoCobranca.Cidade;
        var estadoCliente = cliente.EnderecoCobranca.UF;
        string cidadeEstado = cidadeCliente + " (" + estadoCliente + ")";
        InformacoesAdicionais informacoesAdicionais = new(cidadeEstado, _informacoesAdicionais.CodigoCategoria, _informacoesAdicionais.ContaCorrente);

        int sequenciaDoItem = 1;
        ServicosPrestados servicosPrestados = new(1, cacamba.nCodServ, cacamba.Preco, sequenciaDoItem);
        string enviarNotaFiscal = "S";
        string enviarBoleto = "S";
        string naoEnviarBoleto = "S";

        Email email = (request.TipoDePagamento is TipoDePagamento.Boleto) ?
        new(enviarNotaFiscal, enviarBoleto, cliente.Email)
        : new(enviarNotaFiscal, naoEnviarBoleto, cliente.Email);

        OmieCriarPedidoRequest omieCriarPedido = new(cabecalho, informacoesAdicionais, servicosPrestados, email);
        _logger.LogInformation(@"
            **********Cadastrando pedido na Omie**********");
        var omieResponse = await _mediator.Send(omieCriarPedido);
        if (!omieResponse.Sucesso)
        {
            _logger.LogError(@"
            **********Processo de cadastro do pedido na Omie falhou. Erro: {0}**********", new { omieResponse });
            return omieResponse;
        }
        #endregion

        #region  localRequest
        Int64.TryParse(omieResponse.Mensagem, out long nCodOs);
        PedidoItem pedidoItem = new(cacamba.Volume, cacamba.Preco);
        var pedido = new Pedido(cliente,
                                pedidoItem,
                                enderecoEntrega,
                                request.TipoDePagamento,
                                request.Observacao,
                                cacamba.Preco,
                                codigoIntegracaoOmie,
                                nCodOs);

        #endregion

        #region enviarPedidoParaFila
        #endregion
        _logger.LogInformation(@"
            **********Cadastrando cliente no banco de dados local**********");
        await _pedidoRepositorio.IncluirPedidoAsync(pedido);

        _logger.LogInformation("**********Processo de criacao de pedido concluído com sucesso.**********");
        return omieResponse;
    }
}
