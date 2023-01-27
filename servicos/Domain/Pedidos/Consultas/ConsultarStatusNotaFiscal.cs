using Domain.Compartilhado;
using Domain.Omie.Pedidos;
using Domain.Omie.Pedidos.OmiePedidoRequests;
using Domain.Pedidos.Enumeraveis;
using Domain.Pedidos.Interface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Pedidos.Consultas;

public record ConsultarStatusNotaFiscalQuery(int PedidoId) : IRequest<Resposta>;
public class ConsultarStatusNotaFiscalHandler : IRequestHandler<ConsultarStatusNotaFiscalQuery, Resposta>
{
    private readonly IPedidoRepositorio _pedidoRepositorio;
    private readonly IMediator _mediator;
    private readonly ILogger<ConsultarStatusNotaFiscalHandler> _logger;

    public ConsultarStatusNotaFiscalHandler(IPedidoRepositorio pedidoRepositorio,
                                            IMediator mediator,
                                            ILogger<ConsultarStatusNotaFiscalHandler> logger)
    {
        _pedidoRepositorio = pedidoRepositorio;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Resposta> Handle(ConsultarStatusNotaFiscalQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("**********Processo para consultar status do pedido iniciado.**********");

        if (request is null)
        {
            _logger.LogError(@"
            **********Request não pode ser nulo**********");
            return new("Request não pode ser nulo", false);
        }

        var pedido = await _pedidoRepositorio.ObterPedidoPorIdAsync(request.PedidoId);
        if (pedido is null)
        {
            _logger.LogError(@"
            **********Pedido não encontrado**********");
            return new("Pedido não encontrado", false);
        }

        string cCodIntOS = pedido.cCodIntOS.ToString();
        OmieConsultarStatusPedidoRequest omieConsultarStatusPedido = new(cCodIntOS, pedido.nCodOS);
        var omieResponse = await _mediator.Send(omieConsultarStatusPedido);
        if (!omieResponse.Sucesso)
        {
            _logger.LogError(@"
            **********Processo para consultar status do pedido na Omie falhou. Erro: {0}**********", new { omieResponse });
            return omieResponse;
        }

        if (!string.IsNullOrEmpty(omieResponse.Mensagem))
        {
            _logger.LogInformation("**********Nota fiscal emitida. Salvando número da nota fiscal no pedido e atualizando status**********");
            pedido.AtibuirNumeroNotaFiscal(omieResponse.Mensagem);
            pedido.NotaFiscal.AtualizarStatus(StatusPedido.Concluido, "Nota fiscal emitida");
            await _pedidoRepositorio.AtualizarPedidoAsync(pedido);
        }

        _logger.LogInformation("**********Processo para consultar status do pedido concluído com sucesso.**********");
        return omieResponse;
    }
}
