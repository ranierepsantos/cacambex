using Domain.Compartilhado;
using Domain.Omie.Pedidos.Interface;
using MediatR;

namespace Domain.Omie.Pedidos.OmiePedidoRequests;

public record OmieFaturarPedidoRequest(string cCodIntOS, long nCodOS) : IRequest<Resposta>;
public class OmieFaturarPedidoHandler : IRequestHandler<OmieFaturarPedidoRequest, Resposta>
{
    private readonly IOmiePedidos _pedidos;
    private readonly OmieConfigurations _configurations;

    public OmieFaturarPedidoHandler(IOmiePedidos pedidos, OmieConfigurations configurations)
    {
        _pedidos = pedidos;
        _configurations = configurations;
        _configurations.OMIE_CALL = "FaturarOS";
    }

    public async Task<Resposta> Handle(OmieFaturarPedidoRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
            return new("request não pode ser nulo.", false);

        var body = new OmieRequest(
            call: $"{_configurations.OMIE_CALL}",
            app_key: $"{_configurations.APP_KEY}",
            app_secret: $"{_configurations.APP_SECRET}",
            new() { request });

        var result = await _pedidos.FaturarPedido(body);
        return result;
    }
}
