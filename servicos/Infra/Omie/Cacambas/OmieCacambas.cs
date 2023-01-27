using Domain.Omie;
using Domain.Cacambas.Interface;
using Domain.Compartilhado;
using Domain.Omie.Cacambas.Interfaces;
using Domain.Omie.Cacambas.OmieCacambaResults;
using Flurl;
using Flurl.Http;

namespace infra.Omie.Cacambas
{
    public class OmieCacambas : IOmieCacambas
    {
        private readonly OmieConfigurations _configurations;
        private readonly ICacambaRepositorio _cacambaRepositorio;

        public OmieCacambas(OmieConfigurations configurations, ICacambaRepositorio cacambaRepositorio)
        {
            _configurations = configurations;
            _cacambaRepositorio = cacambaRepositorio;
        }

        public async Task<Resposta> Create(OmieRequest request)
        {
            try
            {
                var httpResult = await _configurations.OMIE_URL
                 .AppendPathSegment("servicos/servico/")
                 .WithHeader("Content-type", "application/json")
                 .WithHeader("accept", "application/json")
                 .PostJsonAsync(request)
                 .ReceiveJson<OmieCriarServicoResult>();

                return new("", true, new OmieCriarServicoResult(httpResult.cCodIntServ, httpResult.nCodServ));
            }
            catch (FlurlHttpException ex)
            {
                var errors = await ex.GetResponseJsonAsync<OmieErrorResult>();
                return new("", false, errors.faultstring);
            }
        }

        public async Task<Resposta> Update(OmieRequest request)
        {
            try
            {
                var httpResult = await _configurations.OMIE_URL
                 .AppendPathSegment("servicos/servico/")
                 .WithHeader("Content-type", "application/json")
                 .WithHeader("accept", "application/json")
                 .PostJsonAsync(request)
                 .ReceiveJson<OmieCriarServicoResult>();

                return new("", true, new OmieCriarServicoResult(httpResult.cCodIntServ, httpResult.nCodServ));

            }
            catch (FlurlHttpException ex)
            {
                var errors = await ex.GetResponseJsonAsync<OmieErrorResult>();
                return new("", false, errors.faultstring);
            }
        }
    }
}