using Domain.Compartilhado;
using Domain.Omie.Pedidos.OmiePedidoRequests;
using Domain.Pedidos.Enumeraveis;
using Domain.Pedidos.Interface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Pedidos.Comandos;

public record EmitirNotaFiscalComando(int PedidoId) : IRequest<Resposta>;
public class EmitirNotaFiscalHandler : IRequestHandler<EmitirNotaFiscalComando, Resposta>
{
    private readonly ILogger<EmitirNotaFiscalHandler> _logger;
    private readonly IPedidoRepositorio _pedidoRepositorio;
    private readonly IMediator _mediator;
    public EmitirNotaFiscalHandler(IPedidoRepositorio pedidoRepositorio,
                                   IMediator mediator,
                                   ILogger<EmitirNotaFiscalHandler> logger)
    {
        _pedidoRepositorio = pedidoRepositorio;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Resposta> Handle(EmitirNotaFiscalComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("**********Processo para emitir NF/faturar pedido iniciado.**********");

        if (request is null)
        {
            _logger.LogError(@"
            **********Request nao pode ser nulo**********");
            return new("Request nao pode ser nulo", false);
        }

        var pedido = await _pedidoRepositorio.ObterPedidoPorIdAsync(request.PedidoId);
        if (pedido is null)
        {
            _logger.LogError(@"
            **********Pedido nao encontrado**********");
            return new("Pedido nao encontrado", false);
        }

        string cCodIntOS = pedido.cCodIntOS.ToString();
        OmieFaturarPedidoRequest omieFaturarPedido = new(cCodIntOS, pedido.nCodOS);
        var omieResponse = await _mediator.Send(omieFaturarPedido);
        if (!omieResponse.Sucesso)
        {
            _logger.LogError(@"
            **********Processo para emitir NF/faturar pedido na Omie falhou. Erro: {0}**********", new { omieResponse });
            return omieResponse;
        }

        pedido.NotaFiscal.AtualizarStatus(StatusPedido.Aguardando, "Aguardando resposta prefeitura");
        await _pedidoRepositorio.AtualizarPedidoAsync(pedido);

        _logger.LogInformation(@"**********Processo para emitir NF/faturar pedido concluido com sucesso. 
        Aguardando resposta da prefeitura.**********");
        return new("Pedido de emissao de nota fiscal enviado. Aguardando resposta da prefeitura.");
    }
}
