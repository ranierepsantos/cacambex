using Domain.Compartilhado;
using Domain.Omie.Cacambas.Interfaces;
using MediatR;

namespace Domain.Omie.Cacambas.OmieCacambaRequest;

public record OmieAlterarCacambaRequest(IntEditar intEditar, CabecalhoAtualizar cabecalho) : IRequest<Resposta>;
public record CabecalhoAtualizar(string cCodigo, decimal nPrecoUnit);
public record IntEditar(string cCodIntServ, double nCodServ);
public class OmieAlterarCacambaHandler : IRequestHandler<OmieAlterarCacambaRequest, Resposta>
{
    private readonly IOmieCacambas _cacambas;
    private readonly OmieConfigurations _configurations;

    public OmieAlterarCacambaHandler(IOmieCacambas cacambas, OmieConfigurations configurations)
    {
        _cacambas = cacambas;
        _configurations = configurations;
        _configurations.OMIE_CALL = "AlterarCadastroServico";
    }
    public async Task<Resposta> Handle(OmieAlterarCacambaRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
            return new("request não pode ser nulo", false);
        var body = new OmieRequest(
            call: $"{_configurations.OMIE_CALL}",
            app_key: $"{_configurations.APP_KEY}",
            app_secret: $"{_configurations.APP_SECRET}",
            new() { request });

        var result = await _cacambas.Update(body);
        return result;
    }
}
