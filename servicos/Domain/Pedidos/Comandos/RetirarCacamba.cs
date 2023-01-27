using Domain.AzureStorage;
using Domain.ColetasOnline;
using Domain.Compartilhado;
using Domain.Pedidos.Interface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Pedidos.Comandos;

public record RetirarCacambaComando(int PedidoId, string PlacaVeiculo) : IRequest<Resposta>;
public class RetirarCacambaHandler : IRequestHandler<RetirarCacambaComando, Resposta>
{
    private readonly IPedidoRepositorio _pedidoRepositorio;
    private readonly ILogger<RetirarCacambaHandler> _logger;
    private readonly IFilaRetirarCacambaRepositorio _queueRepositorio;

    public RetirarCacambaHandler(IPedidoRepositorio pedidoRepositorio,
                                 ILogger<RetirarCacambaHandler> logger,
                                 IFilaRetirarCacambaRepositorio queueRepositorio)
    {
        _pedidoRepositorio = pedidoRepositorio;
        _logger = logger;
        _queueRepositorio = queueRepositorio;
    }

    public async Task<Resposta> Handle(RetirarCacambaComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("**********Processo para retirar cacamba da obra iniciado.**********");
        var pedido = await _pedidoRepositorio.ObterPedidoPorIdAsync(request.PedidoId);
        if (pedido is null)
        {
            _logger.LogError("**********Pedido nao encontrado.**********");
            return new Resposta("Pedido nao encontrado.", false);
        }

        string placaCompleta = ObterPlaca(request);
        DateTime dataRetirada = DateTime.Now;
        RetirarCacambaRequest retirarCacambaRequest = new(
            iCodCidade: 25,
            stNumeroCTR: pedido.NumeroCTR,
            stDataRetirada: dataRetirada.ToShortDateString(),
            stPlacaVeiculo: placaCompleta,
            idDestino: 44,
            CTR_Id: pedido.PedidoItem.CTR.Id,
            RecolherItem_Id: pedido.PedidoItem.RecolherItem.Id,
            PedidoConcluido_Id: pedido.PedidoItem.PedidoConcluido.Id,
            Cacamba_Id: pedido.PedidoItem.Cacamba.Id);
        try
        {
            await _queueRepositorio.FilaRetiraCacamba(retirarCacambaRequest);
            _logger.LogInformation("**********Solicitacao de retirada da cacamba enviada para fila de processamento. Aguarde.**********");
        }
        catch (Exception ex)
        {
            _logger.LogError(@$"**********ERRO ao enviar requisicao para fila de processamento ou ao atualizar pedido no banco: 
                                Exception: {ex.Message}**********");
            throw;
        }
        return new Resposta("Solicitacao de retirada da cacamba enviada com sucesso. Aguarde.");
    }
    private static string ObterPlaca(RetirarCacambaComando request)
    {
        var letras = request.PlacaVeiculo.Substring(0, 3);
        var numeros = request.PlacaVeiculo.Substring(3, 4);
        var placaCompleta = letras + "-" + numeros;
        return placaCompleta.ToUpper();
    }
}