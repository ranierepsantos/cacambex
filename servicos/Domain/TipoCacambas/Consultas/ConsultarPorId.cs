using Domain.Compartilhado;
using Domain.TipoCacambas.Interface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.TipoCacambas.Consultas
{
    public record ConsultarPorIdComando(int id) : IRequest<Resposta>;
    public sealed class ConsultarPorIdHandler : IRequestHandler<ConsultarPorIdComando, Resposta>
    {

        private readonly ILogger<ConsultarPorIdHandler> _logger;
        private readonly ITipoCacambaRepositorio _repositorio;

        public ConsultarPorIdHandler(ILogger<ConsultarPorIdHandler> logger,
                                     ITipoCacambaRepositorio repositorio)
        {
            _logger = logger;
            _repositorio = repositorio;
        }

        public async Task<Resposta> Handle(ConsultarPorIdComando request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("********* Retornando os tipo de caçamba por Id, com preços por faixa de cep ************");

            var tipoCacamba = await _repositorio.ObterTipoCacambaPorIdComPrecoFaixaCepAsync(request.id);
            
            if (tipoCacamba == null)
            {
                return new Resposta($"Tipo caçamba com id: {request.id}, não localizado!", false);
            }
            return new Resposta("", true, tipoCacamba);
        }

    }
}
