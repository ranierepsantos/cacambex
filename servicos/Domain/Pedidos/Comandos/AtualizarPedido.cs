using Domain.Cacambas.Interface;
using Domain.Clientes.Interface;
using Domain.Compartilhado;
using Domain.Omie;
using Domain.Omie.Pedidos.OmiePedidoRequests;
using Domain.Pedidos.Enumeraveis;
using Domain.Pedidos.Interface;
using Domain.Pedidos.Validacoes;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Domain.Pedidos.Comandos;

public record AtualizarPedidoComando(
    int PedidoId,
    int EnderecoId,
    string VolumeCacamba,
    decimal Preco,
    TipoDePagamento TipoDePagamento,
    string? Observacao) : IRequest<Resposta>;
public class AtualizarPedido : IRequestHandler<AtualizarPedidoComando, Resposta>
{
    private readonly IClienteRepositorio _clienteRepositorio;
    private readonly ICacambaRepositorio _cacambaRepositorio;
    private readonly IPedidoRepositorio _repositorio;
    private readonly IMediator _mediator;
    private ILogger<AtualizarPedido> _logger;
    private readonly OmieInformacoesAdicionais _informacoesAdicionais;

    public AtualizarPedido(IPedidoRepositorio repositorio,
                           ILogger<AtualizarPedido> logger,
                           IClienteRepositorio clienteRepositorio,
                           ICacambaRepositorio cacambaRepositorio,
                           IOptions<OmieInformacoesAdicionais> informacoesAdicionais,
                           IMediator mediator)
    {
        _repositorio = repositorio;
        _logger = logger;
        _clienteRepositorio = clienteRepositorio;
        _cacambaRepositorio = cacambaRepositorio;
        _informacoesAdicionais = informacoesAdicionais.Value;
        _mediator = mediator;
    }

    public async Task<Resposta> Handle(AtualizarPedidoComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("**********Processo de atualizacao de pedido iniciado.**********");

        #region validacoes
        if (request is null)
        {
            _logger.LogError("**********Request nao pode ser vazio.**********");
            return new("Request nao pode ser vazio.", false);
        }

        var validacao = new AtualizarPedidoValidacao();
        var resultadoValidacao = validacao.Validate(request);
        if (!resultadoValidacao.IsValid)
        {
            _logger.LogError(@"
        **********Processo de atualizacao de pedido falhou devido a erros de validacao**********");
            return new("", false, resultadoValidacao.Errors.Select(x => x.ErrorMessage));
        }

        var pedido = await _repositorio.ObterPedidoPorIdAsync(request.PedidoId);
        if (pedido is null)
        {
            _logger.LogError("**********Pedido nao encontrado.**********");
            return new("Pedido nao encontrado.", false);
        }

        var cacamba = _cacambaRepositorio.ObterPorVolume (request.VolumeCacamba);
        if (cacamba is null)
        {
            _logger.LogError("**********Cacamba nao encontrada.**********");
            return new("Cacamba nao encontrada.", false);
        }

        var enderecoEntrega = _clienteRepositorio.ObterEnderecoEntregaDoCliente(request.EnderecoId);
        if (enderecoEntrega is null)
        {
            _logger.LogError("**********Endereco de entrega nao encontrado.**********");
            return new("Endereco de entrega nao encontrado.", false);
        }
        #endregion

        #region omieRequest

        int nQtdeParc = 1;
        string cEtapa = "10";
        Cabecalho cabecalho = new(pedido.cCodIntOS, pedido.Cliente.Codigo_cliente_omie, nQtdeParc, cEtapa);

        var cidadeCliente = pedido.Cliente.EnderecoCobranca.Cidade;
        var estadoCliente = pedido.Cliente.EnderecoCobranca.UF;
        string cidadeEstado = cidadeCliente + " (" + estadoCliente + ")";
        InformacoesAdicionais informacoesAdicionais = new(cidadeEstado, _informacoesAdicionais.CodigoCategoria, _informacoesAdicionais.ContaCorrente, _informacoesAdicionais.observacao);


        int sequenciaDoItem = 1;
        string acaoItem = "A";
        ServicosPrestados servicosPrestados = new(1, cacamba.nCodServ, request.Preco, sequenciaDoItem, acaoItem);
        OmieAlterarPedidoRequest omieAlterarPedido = new(cabecalho, informacoesAdicionais, servicosPrestados);
        var omieResponse = await _mediator.Send(omieAlterarPedido);
        if (!omieResponse.Sucesso)
        {
            _logger.LogError(@"
            **********Processo de atualizacao do pedido na Omie falhou. Erro: {0}**********", new { omieResponse });
            return omieResponse;
        }
        #endregion

        #region localRequest
        pedido.PedidoItem.AtualizarPedidoItem(cacamba.Volume, request.Preco);
        pedido.AtualizarPedido(request.Observacao, request.TipoDePagamento, enderecoEntrega, request.Preco);
        await _repositorio.AtualizarPedidoAsync(pedido);

        #endregion

        _logger.LogInformation("**********Processo para atualizar pedido concluido com sucesso.**********");
        return omieResponse;
    }
}
