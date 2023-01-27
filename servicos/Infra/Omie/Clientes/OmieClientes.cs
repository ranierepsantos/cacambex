using Domain.Omie;
using Domain.Omie.Clientes.Interfaces;
using Domain.Clientes.Interface;
using Domain.Compartilhado;
using Domain.Omie.Clientes.OmieClienteResults;
using Flurl;
using Flurl.Http;

namespace infra.Omie.Clientes
{
    public class OmieClientes : IOmieClientes
    {
        private readonly OmieConfigurations _configurations;
        private readonly IClienteRepositorio _clienteRepositorio;

        public OmieClientes(OmieConfigurations configurations, IClienteRepositorio clienteRepositorio)
        {
            _configurations = configurations;
            _clienteRepositorio = clienteRepositorio;
        }

        public async Task<Resposta> OmieCriar(OmieRequest request)
        {
            try
            {
                var httpResult = await _configurations.OMIE_URL
                 .AppendPathSegment("geral/clientes/")
                 .WithHeader("Content-type", "application/json")
                 .WithHeader("accept", "application/json")
                 .PostJsonAsync(request)
                 .ReceiveJson<OmieCriarClienteResult>();

                return new(httpResult.codigo_cliente_omie.ToString());
            }
            catch (FlurlHttpException ex)
            {
                var errors = await ex.GetResponseJsonAsync<OmieErrorResult>();
                return new(errors.faultstring, false);
            }
        }
        public async Task<Resposta> OmieAtualizar(OmieRequest request)
        {
            try
            {
                var httpResult = await _configurations.OMIE_URL
                 .AppendPathSegment("geral/clientes/")
                 .WithHeader("Content-type", "application/json")
                 .WithHeader("accept", "application/json")
                 .PostJsonAsync(request)
                 .ReceiveJson<OmieOperacaoSucessoClienteResult>();

                return new("", true, httpResult);

            }
            catch (FlurlHttpException ex)
            {
                var errors = await ex.GetResponseJsonAsync<OmieErrorResult>();
                return new(errors.faultstring, false);
            }
        }

    }
}