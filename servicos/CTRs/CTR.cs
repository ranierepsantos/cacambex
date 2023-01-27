using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Azure.Storage.Queues;
using CTRs.Data;
using CTRs.Models.RetirarCacamba;
using CTRs.Repositorios;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CTRs;

public class CTR
{
    private static string _url = "http://wscoletas.coletasonline.com.br/WSColetas.asmx";
    private static string _connString;
    private static HttpClient _httpClient;
    private static QueueClient _queueClient;
    private readonly IOptions<MinhasConfiguracoes> _coletasOnlineContext;
    private readonly IClienteRepositorio _clienteRepositorio;
    public CTR(
        HttpClient httpClient,
        IOptions<MinhasConfiguracoes> coletasOnlineContext,
        IClienteRepositorio clienteRepositorio)
    {
        _httpClient = httpClient;
        _coletasOnlineContext = coletasOnlineContext;
        _clienteRepositorio = clienteRepositorio;
    }
    [FunctionName("SolicitaCTR")]
    public async Task SolicitarCTR([QueueTrigger("solicitactr", Connection = "connString")] string myQueueItem,
                                   ILogger log)
    {
        CTRs.Models.SolicitarCTR.SolicitaCTRRequest solicitaCTR = JsonSerializer.Deserialize<CTRs.Models.SolicitarCTR.SolicitaCTRRequest>(myQueueItem);
        var response = await SolicitaCTR(
                                          solicitaCTR, 
                                          _coletasOnlineContext.Value.UsuarioColetasOnline, 
                                          _coletasOnlineContext.Value.SenhaColetasOnline);

        if(response.Body.SolicitaCTRResponse.SolicitaCTRResult.resultado.codigo is not 0)
        {
            _connString = GetEnvironmentVariable("connString");
            _queueClient = new QueueClient(_connString, "solicitactr", new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64,
            });
            await Task.Delay(30000);
            await _clienteRepositorio.SalvarNumeroCTRComErro(solicitaCTR.CTR_Id, response.Body.SolicitaCTRResponse.SolicitaCTRResult.resultado.mensagem);
            await _queueClient.SendMessageAsync(myQueueItem);
            log.LogInformation("Pedido de solicitacão de CTR falhou. Solicitacao reenviada para fila.");
        }
        else
        {
            await _clienteRepositorio.SalvarNumeroCTRComSucesso(solicitaCTR.CTR_Id,
                                                                response.Body.SolicitaCTRResponse.SolicitaCTRResult.resultado.mensagem,
                                                                solicitaCTR.PedidoId,
                                                                response.Body.SolicitaCTRResponse.SolicitaCTRResult.resultado.ID_CTR);
            log.LogInformation("Pedido de solicitacão de CTR processado:", new { response.Body.SolicitaCTRResponse.SolicitaCTRResult.resultado.mensagem });
        }


    }
    private static async Task<CTRs.Models.SolicitarCTR.Envelope> SolicitaCTR(CTRs.Models.SolicitarCTR.SolicitaCTRRequest request, string usuario, string senha)
    {
        var xmlSOAP = @$"<soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
                                <soap12:Body>
                                    <SolicitaCTR xmlns=""http://tempuri.org/"">
                                      <iCodCidade>{request.iCodCidade}</iCodCidade>
                                      <stLoginUser>{usuario}</stLoginUser>
                                      <stSenhaUser>{senha}</stSenhaUser>
                                      <iTipoVeiculo>{request.iTipoVeiculo}</iTipoVeiculo>
                                      <iClassificacao>{request.iClassificacao}</iClassificacao>
                                      <iClasse>{request.iClasse}</iClasse>
                                      <ivolume>{request.ivolume}</ivolume>
                                      <iGGTipo>{request.iGGTipo}</iGGTipo>
                                      <GG_CPF>{request.GG_CPF}</GG_CPF>
                                      <GG_Nome>{request.GG_Nome}</GG_Nome>
                                      <GG_Email>{request.GG_Email}</GG_Email>
                                      <GG_Endereco_CEP>{request.GG_Endereco_CEP}</GG_Endereco_CEP>
                                      <GG_Endereco_Rua>{request.GG_Endereco_Rua}</GG_Endereco_Rua>
                                      <GG_Endereco_Num>{request.GG_Endereco_Num}</GG_Endereco_Num>
                                      <GG_Endereco_Compl>{request.GG_Endereco_Compl}</GG_Endereco_Compl>
                                      <GG_Endereco_Bairro>{request.GG_Endereco_Bairro}</GG_Endereco_Bairro>
                                      <GG_Endereco_Cidade>{request.GG_Endereco_Cidade}</GG_Endereco_Cidade>
                                      <CTR_CEP>{request.CTR_CEP}</CTR_CEP>
                                      <CTR_Rua>{request.CTR_Rua}</CTR_Rua>
                                      <CTR_Num>{request.CTR_Num}</CTR_Num>
                                      <CTR_Compl>{request.CTR_Compl}</CTR_Compl>
                                      <CTR_Bairro>{request.CTR_Bairro}</CTR_Bairro>
                                      <CTR_Cidade>{request.CTR_Cidade}</CTR_Cidade>
                                    </SolicitaCTR>
                                </soap12:Body>
                            </soap12:Envelope>";
        try
        {
            var xmlResult = await PostSOAPRequestAsync(_url, xmlSOAP, "SolicitaCTR");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CTRs.Models.SolicitarCTR.Envelope));
            using (StringReader reader = new StringReader(xmlResult))
            {
                CTRs.Models.SolicitarCTR.Envelope ctrResult = (CTRs.Models.SolicitarCTR.Envelope)xmlSerializer.Deserialize(reader);
                return ctrResult;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

    }
  
    [FunctionName("EnviaCacambaObra")]
    public async Task EnviaCacambaObra([QueueTrigger("enviarcacamba", Connection = "connString")] string myQueueItem, ILogger log)
    {
        CTRs.Models.EnviarCacambaObra.EnviarCacambaObraRequest enviaCacamba = JsonSerializer.Deserialize<CTRs.Models.EnviarCacambaObra.EnviarCacambaObraRequest>(myQueueItem);
        var response = await EnviarCacambaObra(
                                          enviaCacamba,
                                          _coletasOnlineContext.Value.UsuarioColetasOnline,
                                          _coletasOnlineContext.Value.SenhaColetasOnline);

        if (response.Body.EnviaCacambaObra_LocalResponse.EnviaCacambaObra_LocalResult.resultado.codigo is not 0)
        {
            _connString = GetEnvironmentVariable("connString");
            _queueClient = new QueueClient(_connString, "enviarcacamba", new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });
            await Task.Delay(30000);
            await _queueClient.SendMessageAsync(myQueueItem);
            await _clienteRepositorio.SalvarNumeroCTRComErro(enviaCacamba.CTR_Id,
                                                             response.Body.EnviaCacambaObra_LocalResponse.EnviaCacambaObra_LocalResult.resultado.mensagem);
            log.LogInformation("Pedido de envio de cacamba falhou. Solicitacao reenviada para fila.", new { response.Body.EnviaCacambaObra_LocalResponse.EnviaCacambaObra_LocalResult.resultado.mensagem });
        }
        else
        {
            await _clienteRepositorio.SetStatusAguardandoParaRecolherCacamba(enviaCacamba.RecolherItem_Id,
                                                           response.Body.EnviaCacambaObra_LocalResponse.EnviaCacambaObra_LocalResult.resultado.mensagem);
            log.LogInformation("Pedido de envio de cacamba processado:", new { response.Body.EnviaCacambaObra_LocalResponse.EnviaCacambaObra_LocalResult.resultado.mensagem });
        }
    }
    private static async Task<CTRs.Models.EnviarCacambaObra.Envelope> EnviarCacambaObra(CTRs.Models.EnviarCacambaObra.EnviarCacambaObraRequest request,
                                                                                        string usuario,
                                                                                        string senha)
    {
        var xmlSOAP = @$"<soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
                                <soap12:Body>
                                    <EnviaCacambaObra_Local xmlns=""http://tempuri.org/"">
                                      <iCodCidade>{request.iCodCidade}</iCodCidade>
                                      <stLoginUser>{usuario}</stLoginUser>
                                      <stSenhaUser>{senha}</stSenhaUser>
                                      <stNumeroCTR>{request.stNumeroCTR}</stNumeroCTR>
                                      <stDataEnvio>{request.stDataEnvio}</stDataEnvio>
                                      <stPlacaVeiculo>{request.stPlacaVeiculo}</stPlacaVeiculo>
                                      <stIDentificacaoCacamba>{request.stIDentificacaoCacamba}</stIDentificacaoCacamba>
                                      <stLocalEstacionamento>{request.stLocalEstacionamento}</stLocalEstacionamento>
                                    </EnviaCacambaObra_Local>
                                </soap12:Body>
                            </soap12:Envelope>";
        try
        {
            var xmlResult = await PostSOAPRequestAsync(_url, xmlSOAP, "EnviaCacambaObra_Local");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CTRs.Models.EnviarCacambaObra.Envelope));
            using (StringReader reader = new StringReader(xmlResult))
            {
                CTRs.Models.EnviarCacambaObra.Envelope enviaCacambaResult = (CTRs.Models.EnviarCacambaObra.Envelope)xmlSerializer.Deserialize(reader);
                return enviaCacambaResult;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    [FunctionName("RetirarCacambaObra")]
    public async Task RetirarCacamba([QueueTrigger("retirarcacamba", Connection = "connString")] string myQueueItem, ILogger log)
    {
        CTRs.Models.RetirarCacamba.RetirarCacambaObraRequest retirarCacamba = JsonSerializer.Deserialize<CTRs.Models.RetirarCacamba.RetirarCacambaObraRequest>(myQueueItem);
        var response = await RetirarCacambaObra(
                                          retirarCacamba,
                                          _coletasOnlineContext.Value.UsuarioColetasOnline,
                                          _coletasOnlineContext.Value.SenhaColetasOnline);

        if (response.Body.RetirarCacambaObraResponse.RetirarCacambaObraResult.resultado.codigo is not 0)
        {
            _connString = GetEnvironmentVariable("connString");
            _queueClient = new QueueClient(_connString, "retirarcacamba", new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });
            //await Task.Delay(30000);
            await _queueClient.SendMessageAsync(myQueueItem);
            await _clienteRepositorio.SalvarNumeroCTRComErro(retirarCacamba.CTR_Id,
                                                             response.Body.RetirarCacambaObraResponse.RetirarCacambaObraResult.resultado.mensagem);
            log.LogInformation("Pedido de retirada de cacamba falhou. Solicitacao reenviada para fila.", new { response.Body.RetirarCacambaObraResponse.RetirarCacambaObraResult.resultado.mensagem });
        }
        else
        {
            await _clienteRepositorio.SetStatusConcluidoParaRecolherCacambaEPedidoConcluido(retirarCacamba.RecolherItem_Id,
                                                           retirarCacamba.PedidoConcluido_Id,
                                                           retirarCacamba.Cacamba_Id,
                                                           response.Body.RetirarCacambaObraResponse.RetirarCacambaObraResult.resultado.mensagem);
            log.LogInformation("Pedido de retirada de cacamba processado:", new { response.Body.RetirarCacambaObraResponse.RetirarCacambaObraResult.resultado.mensagem });
        }
    }
    private static async Task<CTRs.Models.RetirarCacamba.Envelope> RetirarCacambaObra(RetirarCacambaObraRequest request, string usuario, string senha)
    {
        var xmlSOAP = @$"<soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
                                <soap12:Body>
                                    <RetirarCacambaObra xmlns=""http://tempuri.org/"">
                                      <iCodCidade>{request.iCodCidade}</iCodCidade>
                                      <stLoginUser>{usuario}</stLoginUser>
                                      <stSenhaUser>{senha}</stSenhaUser>
                                      <stNumeroCTR>{request.stNumeroCTR}</stNumeroCTR>
                                      <stDataRetirada>{request.stDataRetirada}</stDataRetirada>
                                      <stPlacaVeiculo>{request.stPlacaVeiculo}</stPlacaVeiculo>
                                      <idDestino>{request.idDestino}</idDestino>
                                    </RetirarCacambaObra>
                                </soap12:Body>
                            </soap12:Envelope>";
        try
        {
            var xmlResult = await PostSOAPRequestAsync(_url, xmlSOAP, "RetirarCacambaObra");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CTRs.Models.RetirarCacamba.Envelope));
            using (StringReader reader = new StringReader(xmlResult))
            {
                CTRs.Models.RetirarCacamba.Envelope retirarCacambaResult = (CTRs.Models.RetirarCacamba.Envelope)xmlSerializer.Deserialize(reader);
                return retirarCacambaResult;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    private static async Task<string> PostSOAPRequestAsync(string url, string text, string endpoint)
    {
        using (HttpContent content = new StringContent(text, Encoding.UTF8, "text/xml"))
        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
        {
            request.Headers.Add("SOAPAction", $"http://tempuri.org/{endpoint}");

            request.Content = content;
            using (HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                //response.EnsureSuccessStatusCode(); // throws an Exception if 404, 500, etc.
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
    private static string GetEnvironmentVariable(string nome)
    {
        return nome + ": " +
            System.Environment.GetEnvironmentVariable(nome, EnvironmentVariableTarget.Process);
    }
}
