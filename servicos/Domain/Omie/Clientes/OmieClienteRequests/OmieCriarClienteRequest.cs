using Domain.Compartilhado;
using Domain.Omie.Clientes.Interfaces;
using Domain.Omie.Clientes.OmieClienteResults;
using MediatR;

namespace Domain.Omie.Clientes.OmieClienteRequests;
public record OmieCriarClienteRequest : IRequest<Resposta>
{
    public OmieCriarClienteRequest(
        string codigo_cliente_integracao,
        string email,
        string razao_social,
        string cnpj_cpf,
        string telefone1_numero,
        string endereco,
        string endereco_numero,
        string bairro,
        string estado,
        string cidade,
        string cep,
        string contribuinte,
        string pessoa_fisica,
        List<EnderecoEntregaOmie> enderecoEntrega)
    {
        this.codigo_cliente_integracao = codigo_cliente_integracao;
        this.email = email;
        this.razao_social = razao_social;
        this.cnpj_cpf = cnpj_cpf;
        this.telefone1_numero = telefone1_numero;
        this.endereco = endereco;
        this.endereco_numero = endereco_numero;
        this.bairro = bairro;
        this.estado = estado.ToUpper();
        this.cidade = cidade;
        this.cep = cep;
        this.contribuinte = contribuinte.ToUpper();
        this.pessoa_fisica = pessoa_fisica.ToUpper();
        this.enderecoEntrega = enderecoEntrega;
    }

    public string codigo_cliente_integracao { get; init; }
    public string email { get; init; }
    public string razao_social { get; init; }
    public string cnpj_cpf { get; init; }
    public string telefone1_numero { get; init; }
    public string endereco { get; init; }
    public string endereco_numero { get; init; }
    public string bairro { get; init; }
    public string estado { get; init; }
    public string cidade { get; init; }
    public string cep { get; init; }
    public string contribuinte { get; init; }
    public string pessoa_fisica { get; init; }
    public List<EnderecoEntregaOmie> enderecoEntrega { get; init; }
}
public class OmieCriarClienteHandler : IRequestHandler<OmieCriarClienteRequest, Resposta>
{
    private readonly IOmieClientes _clientes;
    private readonly OmieConfigurations _configurations;

    public OmieCriarClienteHandler(IOmieClientes clientes, OmieConfigurations configurations)
    {
        _clientes = clientes;
        _configurations = configurations;
        _configurations.OMIE_CALL = "IncluirCliente";
    }
    public async Task<Resposta> Handle(OmieCriarClienteRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
            return new("request não pode ser nulo", false);

        var body = new OmieRequest(
            call: $"{_configurations.OMIE_CALL}",
            app_key: $"{_configurations.APP_KEY}",
            app_secret: $"{_configurations.APP_SECRET}",
            new() { request });

        var result = await _clientes.OmieCriar(body);
        return result;
    }
}