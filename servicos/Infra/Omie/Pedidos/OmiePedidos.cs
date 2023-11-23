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
        // Consultar a API para obter informações do serviço
        var consultaApiResult = await ConsultarApiServico(request.param[0]["servicosPrestados"]["nCodServico"]);

        // Verificar se a consulta foi bem-sucedida
        if (consultaApiResult != null)
        {
            // Adicionar as informações de imposto ao objeto request
            request.param[0]["servicosPrestados"]["impostos"] = new
            {
                cRetemIRRF = consultaApiResult["impostos"]["cRetIR"],
                cRetemPIS = consultaApiResult["impostos"]["cRetPIS"],
                nAliqCOFINS = consultaApiResult["impostos"]["nAliqCOFINS"],
                nAliqCSLL = consultaApiResult["impostos"]["nAliqCSLL"],
                nAliqIRRF = consultaApiResult["impostos"]["nAliqIR"],
                nAliqISS = consultaApiResult["impostos"]["nAliqISS"],
                nAliqPIS = consultaApiResult["impostos"]["nAliqPIS"]
            };
        }

        // Realizar a requisição POST
        var httpResult = await _configurations.OMIE_URL
            .AppendPathSegment("servicos/os/")
            .WithHeader("Content-type", "application/json")
            .WithHeader("accept", "application/json")
            .PostJsonAsync(request)
            .ReceiveJson<OmieOrdemServicoResult>();

        return new Resposta(httpResult.nCodOS.ToString());
    }
    catch (FlurlHttpException ex)
    {
        var errors = await ex.GetResponseJsonAsync<OmieErrorResult>();
        return new Resposta(errors.faultstring, false);
    }
}

private async Task<ConsultaApiResult> ConsultarApiServico(long nCodServico)
{
    try
    {
        // Fazer a requisição para a API de consulta
        var consultaApiResponse = await "https://app.omie.com.br/api/v1/servicos/servico/"
            .WithHeader("Content-type", "application/json")
            .PostJsonAsync(new
            {
                call = "ConsultarCadastroServico",
                app_key = "1955488711176",
                app_secret = "deaa35aba6ac00e7d1057174b526570f",
                param = new[]
                {
                    new { nCodServ = nCodServico }
                }
            })
            .ReceiveJson<ConsultaApiResult>();

        // Retornar o resultado da consulta
        return consultaApiResponse;
    }
    catch (FlurlHttpException ex)
    {
        // Lidar com exceções ou erros de requisição
        // Aqui você pode tratar erros, logar, ou tomar outras ações necessárias
        return null;
    }
}

// Classes para representar o resultado da consulta à API
public class ConsultaApiResult
{
    public Cabecalho cabecalho { get; set; }
    public Impostos impostos { get; set; }
    public Info info { get; set; }

    // Outras propriedades necessárias podem ser adicionadas conforme necessário
}

public class Cabecalho
{
    public string cCodCateg { get; set; }
    public string cCodLC116 { get; set; }
    public string cCodServMun { get; set; }
    public string cCodigo { get; set; }
    public string cDescricao { get; set; }
    public string cIdTrib { get; set; }
    public string nIdNBS { get; set; }
    public decimal nPrecoUnit { get; set; }
}

public class Impostos
{
    public string cRetCOFINS { get; set; }
    public string cRetCSLL { get; set; }
    public string cRetINSS { get; set; }
    public string cRetIR { get; set; }
    public string cRetISS { get; set; }
    public string cRetPIS { get; set; }
    public bool lDeduzISS { get; set; }
    public decimal nAliqCOFINS { get; set; }
    public decimal nAliqCSLL { get; set; }
    public decimal nAliqINSS { get; set; }
    public decimal nAliqIR { get; set; }
    public decimal nAliqISS { get; set; }
    public decimal nAliqPIS { get; set; }
    public decimal nRedBaseCOFINS { get; set; }
    public decimal nRedBaseINSS { get; set; }
    public decimal nRedBasePIS { get; set; }

    // Outras propriedades necessárias podem ser adicionadas conforme necessário
}

public class Info
{
    public string cImpAPI { get; set; }
    public string dAlt { get; set; }
    public string dInc { get; set; }
    public string hAlt { get; set; }
    public string hInc { get; set; }
    public string inativo { get; set; }
    public string uAlt { get; set; }
    public string uInc { get; set; }

    // Outras propriedades necessárias podem ser adicionadas conforme necessário
}

    // public async Task<Resposta> CriarPedido(OmieRequest request)
    // {
    //     try
    //     {
    //         var httpResult = await _configurations.OMIE_URL
    //          .AppendPathSegment("servicos/os/")
    //          .WithHeader("Content-type", "application/json")
    //          .WithHeader("accept", "application/json")
    //          .PostJsonAsync(request)
    //          .ReceiveJson<OmieOrdemServicoResult>();

    //         return new(httpResult.nCodOS.ToString());
    //     }
    //     catch (FlurlHttpException ex)
    //     {
    //         var errors = await ex.GetResponseJsonAsync<OmieErrorResult>();
    //         return new(errors.faultstring, false);
    //     }
    // }

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
