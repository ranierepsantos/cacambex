using Domain.Cacambas.Interface;
using Domain.Compartilhado;
using Domain.Pedidos.Interface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Pedidos.Comandos;

public record VincularCacambaComando(int PedidoId,
                                     int CacambaId) : IRequest<Resposta>;

public class VincularCacambaManipulador : IRequestHandler<VincularCacambaComando, Resposta>
{
    private readonly IPedidoRepositorio _pedidoRepositorio;
    private readonly ICacambaRepositorio _cacambaRepositorio;
    private readonly ILogger<VincularCacambaManipulador> _logger;
    public VincularCacambaManipulador(IPedidoRepositorio pedidoRepositorio,
        ICacambaRepositorio cacambaRepositorio,
        ILogger<VincularCacambaManipulador> logger)
    {
        _pedidoRepositorio = pedidoRepositorio;
        _cacambaRepositorio = cacambaRepositorio;
        _logger = logger;
    }
    public async Task<Resposta> Handle(VincularCacambaComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(@"
        **********Processo para vincular cacamba ao pedido iniciado**********");

        #region validacoes
        var pedido = await _pedidoRepositorio.ObterPedidoPorIdAsync(request.PedidoId);
        if (pedido is null)
            return new Resposta("Pedido nao encontrado.", false);

        var cacamba = _cacambaRepositorio.ObterPorId(request.CacambaId);
        if (cacamba is null)
            return new Resposta("Cacamba nao encontrada.", false);

        #endregion

        cacamba.AlterarStatus(cacamba);
        pedido.PedidoItem.VincularCacamba(cacamba);

        await _pedidoRepositorio.AtualizarPedidoAsync(pedido);
        await _cacambaRepositorio.AtualizarCacamba(cacamba);

        _logger.LogInformation(@"
        **********Processo para vincular cacamba ao pedido concluido com sucesso**********");
        return new Resposta("Cacamba vinculada com sucesso.");
    }
}

