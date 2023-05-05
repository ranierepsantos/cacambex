using Domain.Compartilhado;
using Domain.TipoCacambas.Agregacao;
using Domain.TipoCacambas.Interface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.TipoCacambas.Consultas
{
    public record ListarComPrecoFaixaCepComando(string cep) : IRequest<Resposta>;
    public sealed class ListarComPrecoFaixaCepHandler : IRequestHandler<ListarComPrecoFaixaCepComando, Resposta>
    {

        private readonly ILogger<ListarComPrecoFaixaCepHandler> _logger;
        private readonly ITipoCacambaRepositorio _repositorio;

        public ListarComPrecoFaixaCepHandler(ILogger<ListarComPrecoFaixaCepHandler> logger,
                                         ITipoCacambaRepositorio repositorio)
        {
            _logger = logger;
            _repositorio = repositorio;
        }

        public async Task<Resposta> Handle(ListarComPrecoFaixaCepComando request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("********* Retornando os tipos de caçamba com Preço por faixa de cep ************");

            var query = _repositorio.ToQueryWithPrecoFaixaCepAsNoTracking(request.cep);

            query = query.OrderBy(p => p.Volume);

            var dados = query.ToList();

            Resposta resposta = await Task.FromResult(new Resposta("", true, dados));

            return resposta;
        }

    }
}
