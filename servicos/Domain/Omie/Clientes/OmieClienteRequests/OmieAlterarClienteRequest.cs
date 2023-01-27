using Domain.Compartilhado;
using Domain.Omie.Clientes.Interfaces;
using Domain.Omie.Clientes.OmieClienteResults;
using MediatR;

namespace Domain.Omie.Clientes.OmieClienteRequests;
public record OmieAlterarClienteRequest(
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
        List<EnderecoEntregaOmie> enderecoEntrega
) : IRequest<Resposta>;
public class OmieAlterarClienteHandler : IRequestHandler<OmieAlterarClienteRequest, Resposta>
{
    IOmieClientes _clientes;
    OmieConfigurations _configurations;

    public OmieAlterarClienteHandler(IOmieClientes clientes, OmieConfigurations configurations)
    {
        _clientes = clientes;
        _configurations = configurations;
        _configurations.OMIE_CALL = "UpsertCliente";
    }
    public async Task<Resposta> Handle(OmieAlterarClienteRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
            return new("request não pode ser nulo", false);

        var body = new OmieRequest(
            call: $"{_configurations.OMIE_CALL}",
            app_key: $"{_configurations.APP_KEY}",
            app_secret: $"{_configurations.APP_SECRET}",
            new() { request });

        var result = await _clientes.OmieAtualizar(body);
        return result;
    }
}