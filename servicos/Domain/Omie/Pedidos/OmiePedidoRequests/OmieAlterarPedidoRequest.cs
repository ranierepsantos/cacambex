using Domain.Compartilhado;
using Domain.Omie.Pedidos.Interface;
using MediatR;

namespace Domain.Omie.Pedidos.OmiePedidoRequests;
public record OmieAlterarPedidoRequest(Cabecalho cabecalho, InformacoesAdicionais informacoesAdicionais, ServicosPrestados servicosPrestados) : IRequest<Resposta>;

public class OmieAlterarPedidoHandler : IRequestHandler<OmieAlterarPedidoRequest, Resposta>
{
    private readonly IOmiePedidos _pedidos;
    private readonly OmieConfigurations _configurations;

    public OmieAlterarPedidoHandler(IOmiePedidos pedidos, OmieConfigurations configurations)
    {
        _pedidos = pedidos;
        _configurations = configurations;
        _configurations.OMIE_CALL = "AlterarOS";
    }

    public async Task<Resposta> Handle(OmieAlterarPedidoRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
            return new("Request não pode ser nulo.", false);

        var body = new OmieRequest(
            call: $"{_configurations.OMIE_CALL}",
            app_key: $"{_configurations.APP_KEY}",
            app_secret: $"{_configurations.APP_SECRET}",
            new() { request });

        var result = await _pedidos.AtualizarPedido(body);
        return result;
    }
}