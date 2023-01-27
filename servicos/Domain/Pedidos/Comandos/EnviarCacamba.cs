using Domain.AzureStorage;
using Domain.ColetasOnline;
using Domain.Compartilhado;
using Domain.Pedidos.Interface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Pedidos.Comandos;

public record EnviarCacambaComando(int PedidoId,
                                   string PlacaVeiculo,
                                   string NumeroCacamba,
                                   string LocalEstacionado) : IRequest<Resposta>;
public class EnviarCacambaHandler : IRequestHandler<EnviarCacambaComando, Resposta>
{
    private readonly IPedidoRepositorio _pedidoRepositorio;
    private readonly ILogger<EnviarCacambaHandler> _logger;
    private readonly IFilaEnviarCacambaRepositorio _queueRepositorio;

    public EnviarCacambaHandler(IPedidoRepositorio pedidoRepositorio,
                                 ILogger<EnviarCacambaHandler> logger,
                                 IFilaEnviarCacambaRepositorio queueRepositorio)
    {
        _pedidoRepositorio = pedidoRepositorio;
        _logger = logger;
        _queueRepositorio = queueRepositorio;
    }

    public async Task<Resposta> Handle(EnviarCacambaComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("**********Processo de envio de cacamba iniciado.**********");
        var pedido = await _pedidoRepositorio.ObterPedidoPorIdAsync(request.PedidoId);
        if (pedido is null)
        {
            _logger.LogError("**********Pedido nao encontrado.**********");
            return new Resposta("Pedido nao encontrado.", false);
        }

        string placaCompleta = ObterPlaca(request);

        DateTime dataEnvio = DateTime.Now;
        EnviarCacambaRequest EnviarCacambaRequest = new(
            iCodCidade: 25,
            stNumeroCTR: pedido.NumeroCTR,
            stDataEnvio: dataEnvio.ToShortDateString(),
            stPlacaVeiculo: placaCompleta,
            stIDentificacaoCacamba: request.NumeroCacamba,
            stLocalEstacionamento: request.LocalEstacionado,
            RecolherItem_Id: pedido.PedidoItem.RecolherItem.Id);

        //pedido.PedidoItem.AtualizarCTR("Solicitacao para enviar cacamba a obra enviada para fila de processamento.");
        try
        {
            await _pedidoRepositorio.AtualizarPedidoAsync(pedido);
            await _queueRepositorio.FilaEnviarCacamba(EnviarCacambaRequest);
            _logger.LogInformation("**********Solicitacao para enviar cacamba a obra enviada para fila de processamento.**********");

        }
        catch (Exception ex)
        {
            _logger.LogError(@$"**********ERRO ao enviar requisicao para fila de processamento ou ao atualizar pedido no banco: 
                                Exception: {ex.Message}**********");
            throw;
        }
        return new Resposta("Solicitacao para enviar cacamba a obra enviada para fila de processamento. Aguarde.");
    }

    private static string ObterPlaca(EnviarCacambaComando request)
    {
        var letras = request.PlacaVeiculo.Substring(0, 3);
        var numeros = request.PlacaVeiculo.Substring(3, 4);
        var placaCompleta = letras + "-" + numeros;
        return placaCompleta.ToUpper();
    }
}