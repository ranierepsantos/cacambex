using Domain.Compartilhado;
using Domain.Omie.Cacambas.Interfaces;
using Domain.Omie.Cacambas.OmieCacambaResults;
using MediatR;

namespace Domain.Omie.Cacambas.OmieCacambaRequest;

public record OmieCriarCacambaRequest(IntIncluir intIncluir, Cabecalho cabecalho) : IRequest<Resposta>;
public record IntIncluir(string cCodIntServ);
public class OmieCriarCacambaHandler : IRequestHandler<OmieCriarCacambaRequest, Resposta>
{
    private readonly IOmieCacambas _cacambas;
    private readonly OmieConfigurations _configurations;

    public OmieCriarCacambaHandler(IOmieCacambas cacambas, OmieConfigurations configurations)
    {
        _cacambas = cacambas;
        _configurations = configurations;
        _configurations.OMIE_CALL = "IncluirCadastroServico";
    }
    public async Task<Resposta> Handle(OmieCriarCacambaRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
            return new("request não pode ser nulo.", false);
        var body = new OmieRequest(
            call: $"{_configurations.OMIE_CALL}",
            app_key: $"{_configurations.APP_KEY}",
            app_secret: $"{_configurations.APP_SECRET}",
            new() { request });

        var result = await _cacambas.Create(body);
        return result;
    }
}