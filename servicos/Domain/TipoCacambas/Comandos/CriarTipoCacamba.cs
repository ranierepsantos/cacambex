using Domain.Compartilhado;
using Domain.Identidade.Comandos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.TipoCacambas.Comandos
{
    public record CriarTipoCacambaComando (string volume, decimal preco) : IRequest<Resposta>;
    public sealed class CriarTipoCacambaHandler : IRequestHandler<CriarTipoCacambaComando, Resposta>
    {
        private readonly ILogger<CriarTipoCacambaHandler> _logger;

        public CriarTipoCacambaHandler(ILogger<CriarTipoCacambaHandler> logger)
        {
            _logger = logger;
        }

        public Task<Resposta> Handle(CriarTipoCacambaComando request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("********* não implementado ************");
            throw new NotImplementedException();
        }
    }
}
