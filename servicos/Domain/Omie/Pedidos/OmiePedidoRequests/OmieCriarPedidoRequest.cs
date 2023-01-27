using Domain.Compartilhado;
using Domain.Omie.Pedidos.Interface;
using MediatR;

namespace Domain.Omie.Pedidos.OmiePedidoRequests;
public record OmieCriarPedidoRequest(Cabecalho cabecalho, InformacoesAdicionais informacoesAdicionais, ServicosPrestados servicosPrestados, Email email) : IRequest<Resposta>;
public record Cabecalho(Guid cCodIntOS, long nCodCli, int nQtdeParc, string cEtapa);
public record InformacoesAdicionais(string cCidPrestServ, string cCodCateg, long nCodCC);
public record ServicosPrestados(int nQtde, long nCodServico, decimal nValUnit, int nSeqItem, string? cAcaoItem = null);
public record Email(string cEnvLink, string cEnvBoleto, string cEnviarPara);
public class OmieCriarPedidoHandler : IRequestHandler<OmieCriarPedidoRequest, Resposta>
{
    private readonly IOmiePedidos _pedidos;
    private readonly OmieConfigurations _configurations;

    public OmieCriarPedidoHandler(IOmiePedidos pedidos, OmieConfigurations configurations)
    {
        _pedidos = pedidos;
        _configurations = configurations;
        _configurations.OMIE_CALL = "IncluirOS";
    }

    public async Task<Resposta> Handle(OmieCriarPedidoRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
            return new("request não pode ser nulo.", false);

        var body = new OmieRequest(
            call: $"{_configurations.OMIE_CALL}",
            app_key: $"{_configurations.APP_KEY}",
            app_secret: $"{_configurations.APP_SECRET}",
            new() { request });

        var result = await _pedidos.CriarPedido(body);
        return result;
    }
}