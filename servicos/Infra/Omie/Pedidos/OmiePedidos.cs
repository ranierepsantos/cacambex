using Domain.Omie;
using Domain.Compartilhado;
using Domain.Omie.Pedidos.Interface;
using Domain.Pedidos.Interface;
using Flurl;
using Flurl.Http;
using Domain.Omie.Pedidos;

namespace infra.Omie.Pedidos;
public class OmiePedidos : IOmiePedidos
{
    private readonly OmieConfigurations _configurations;
    public OmiePedidos(IPedidoRepositorio pedidoRepositorio, OmieConfigurations configurations)
    {
        _configurations = configurations;
    }

    public async Task<Resposta> CriarPedido(OmieRequest request)
    {
        try
        {

            var httpTest = await _configurations.OMIE_TEST_URL
             .AppendPathSegment("teste")
             .WithHeader("Content-type", "application/json")
             .WithHeader("accept", "application/json")
             .PostJsonAsync(request)
             .ReceiveJson<OmieOrdemServicoResult>();


            var httpResult = await _configurations.OMIE_URL
             .AppendPathSegment("servicos/os/")
             .WithHeader("Content-type", "application/json")
             .WithHeader("accept", "application/json")
             .PostJsonAsync(request)
             .ReceiveJson<OmieOrdemServicoResult>();

            return new(httpResult.nCodOS.ToString());
        }
        catch (FlurlHttpException ex)
        {
            var errors = await ex.GetResponseJsonAsync<OmieErrorResult>();
            return new(errors.faultstring, false);
        }
    }

    public async Task<Resposta> AtualizarPedido(OmieRequest request)
    {
        try
        {
            var httpResult = await _configurations.OMIE_URL
             .AppendPathSegment("servicos/os/")
             .WithHeader("Content-type", "application/json")
             .WithHeader("accept", "application/json")
             .PostJsonAsync(request)
             .ReceiveJson<OmieOrdemServicoResult>();

            return new("", true, httpResult);
        }
        catch (FlurlHttpException ex)
        {
            var errors = await ex.GetResponseJsonAsync<OmieErrorResult>();
            return new(errors.faultstring, false);
        }
    }

    public async Task<Resposta> FaturarPedido(OmieRequest request)
    {
        try
        {
            var httpResult = await _configurations.OMIE_URL
             .AppendPathSegment("servicos/osp/")
             .WithHeader("Content-type", "application/json")
             .WithHeader("accept", "application/json")
             .PostJsonAsync(request)
             .ReceiveJson<OmieFaturarOSResult>();

            return new("", true, httpResult);
        }
        catch (FlurlHttpException ex)
        {
            var errors = await ex.GetResponseJsonAsync<OmieErrorResult>();
            return new(errors.faultstring, false);
        }
    }

    public async Task<Resposta> ConsultarStatusPedido(OmieRequest request)
    {
        try
        {
            var httpResult = await _configurations.OMIE_URL
             .AppendPathSegment("servicos/os/")
             .WithHeader("Content-type", "application/json")
             .WithHeader("accept", "application/json")
             .PostJsonAsync(request)
             .ReceiveJson<OmieConsultarStatusPedidoResult>();

            List<string> nf = new();
            httpResult.ListaRpsNfse.ForEach(resposta => nf.Add(resposta.nNfse));

            return new(nf[0], true, httpResult.ListaRpsNfse);
        }
        catch (FlurlHttpException ex)
        {
            var errors = await ex.GetResponseJsonAsync<OmieErrorResult>();
            return new(errors.faultstring, false);
        }
    }
}
