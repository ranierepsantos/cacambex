using System.Collections.Generic;
using Domain.Omie;
using Domain.Omie.Clientes.Interfaces;
using Domain.Omie.Clientes.OmieClienteRequests;
using Domain.Omie.Clientes.OmieClienteResults;
using Flurl.Http;
using Flurl.Http.Testing;
using infra.Omie.Clientes;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Priority;

namespace IntegrationTests.Omie;
[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class OmieRequestsTests
{
    private readonly HttpTest _httpTest;
    private readonly EnderecoEntregaOmie _enderecoValido;
    private readonly List<EnderecoEntregaOmie> _listaEndereco;
    private readonly OmieConfigurations _omieConfigurations;
    Mock<IOmieClientes> _omieClientes = new Mock<IOmieClientes>();
    Mock<ILogger<OmieCriarClienteHandler>> _logger = new Mock<ILogger<OmieCriarClienteHandler>>();
    Mock<ILogger<OmieClientes>> _loggerOmieClientes = new Mock<ILogger<OmieClientes>>();

    public OmieRequestsTests()
    {
        _enderecoValido = new EnderecoEntregaOmie(
                entEndereco: "rua teste",
                entNumero: "12",
                entBairro: "bairro teste",
                entCEP: "13219110",
                entEstado: "SP",
                entCidade: "Jundiaí"
        );
        _listaEndereco = new List<EnderecoEntregaOmie>();
        _listaEndereco.Add(_enderecoValido);

        _omieConfigurations = new OmieConfigurations(
            "https://app.omie.com.br/api/v1/geral/clientes/",
            "2699300300697",
            "b7ab98a7fc57e3aba0639bcbf393ff39");

        _httpTest = new HttpTest();
    }
    //[Fact, Priority(1)]
    public async void AoCriarUmClienteNaOmie_ComDadosValidos_RetornarStatusCode200()
    {
        _omieConfigurations.OMIE_CALL = "IncluirCliente";
        _httpTest.ForCallsTo(_omieConfigurations.OMIE_URL).AllowRealHttp();

        OmieCriarClienteRequest clienteRequest = new(
            codigo_cliente_integracao: "40560278896",
            email: "douglas@teste.com",
            razao_social: "douglas teste",
            cnpj_cpf: "40560278896",
            telefone1_numero: "11941012994",
            endereco: "rua teste",
            endereco_numero: "10",
            bairro: "teste",
            estado: "SP",
            cidade: "Jundiaí",
            cep: "13219110",
            contribuinte: "S",
            pessoa_fisica: "S",
            _listaEndereco
        );

        OmieRequest omieRequest = new(
             call: $"{_omieConfigurations.OMIE_CALL}",
                    app_key: $"{_omieConfigurations.APP_KEY}",
                    app_secret: $"{_omieConfigurations.APP_SECRET}",
                    new List<object>() { clienteRequest });

        var httpResult = await "https://app.omie.com.br/api/v1/geral/clientes/"
                .WithHeader("Content-type", "application/json")
                .WithHeader("accept", "application/json")
                .PostJsonAsync(omieRequest);

        var statusCodeResult = httpResult.StatusCode;
        Assert.Equal(200, statusCodeResult);
    }


    [Fact, Priority(4)]
    public async void AoAlterarUmClienteNaOmie_ComDadosValidos_RetornarStatusCode200()
    {
        _omieConfigurations.OMIE_CALL = "UpsertCliente";
        _httpTest.ForCallsTo(_omieConfigurations.OMIE_URL).AllowRealHttp();

        OmieAlterarClienteRequest clienteRequest = new(
            codigo_cliente_integracao: "40560278896",
            email: "douglas@teste.com",
            razao_social: "douglas teste alterado",
            cnpj_cpf: "40560278896",
            telefone1_numero: "11941012994",
            endereco: "rua teste alterado",
            endereco_numero: "10",
            bairro: "teste alterado",
            estado: "SP",
            cidade: "Jundiaí",
            cep: "13219110",
            contribuinte: "S",
            pessoa_fisica: "S",
            _listaEndereco
        );

        OmieRequest omieRequest = new(
             call: $"{_omieConfigurations.OMIE_CALL}",
                    app_key: $"{_omieConfigurations.APP_KEY}",
                    app_secret: $"{_omieConfigurations.APP_SECRET}",
                    new List<object>() { clienteRequest });

        var httpResult = await "https://app.omie.com.br/api/v1/geral/clientes/"
                .WithHeader("Content-type", "application/json")
                .WithHeader("accept", "application/json")
                .PostJsonAsync(omieRequest);

        var statusCodeResult = httpResult.StatusCode;
        Assert.Equal(200, statusCodeResult);
    }

    [Fact]
    public async void FakeTest()
    {
        _httpTest.RespondWith("OK", 200);
        var httpResult = await "http://douglasapi.com".WithHeader("fake", "fake").GetAsync();

        var statusCode = await httpResult.GetStringAsync();
        Assert.Equal(200, httpResult.StatusCode);
        Assert.Equal("OK", statusCode);
    }
}