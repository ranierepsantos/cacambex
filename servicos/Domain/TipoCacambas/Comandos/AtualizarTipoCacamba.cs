using Domain.Cacambas.Agregacao;
using Domain.Cacambas.Validacoes;
using Domain.Compartilhado;
using Domain.TipoCacambas.Agregacao;
using Domain.TipoCacambas.Interface;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.TipoCacambas.Comandos
{
    public record AtualizarTipoCacambaComando(
        int Id,
        string Volume,
        decimal Preco,
        Boolean Ativo) : IRequest<Resposta>;
    public sealed class AtualizarTipoCacambaHandler : IRequestHandler<AtualizarTipoCacambaComando, Resposta>
    {
        private readonly ILogger<AtualizarTipoCacambaHandler> _logger;
        private readonly ITipoCacambaRepositorio _repositorio;

        public AtualizarTipoCacambaHandler(ILogger<AtualizarTipoCacambaHandler> logger, ITipoCacambaRepositorio repositorio)
        {
            _logger = logger;
            _repositorio = repositorio;
        }

        public async Task<Resposta> Handle(AtualizarTipoCacambaComando request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"********* Atualizando tipo cacamba  {request.Id} ************");

            #region validacoes
            if (request is null)
            {
                _logger.LogError(@"**********Request não pode ser nulo.**********");
                return new("Request não pode ser nulo.", false);
            }
            var validacao = new AtualizarTipoCacambaValidacao();
            var resultadoValidacao = validacao.Validate(request);
            if (!resultadoValidacao.IsValid)
            {
                _logger.LogError(@"**********Processo de atualizacao de tipo caçamba falhou devido a erros de validação.**********");
                return new("", false, resultadoValidacao.Errors.Select(x => x.ErrorMessage));

            }

            var tipoCacamba = await _repositorio.ObterTipoCacambaPorIdAsync(request.Id);
            if (tipoCacamba is null)
            {
                _logger.LogError(@"********** Tipo Caçamba não encontrada.**********");
                return new("Tipo Caçamba não encontrada.", false);
            }
            #endregion

            tipoCacamba.AtualizarTipoCacamba(request.Volume, request.Preco, request.Ativo);

            await _repositorio.AtualizarTipoCacambaAsync(tipoCacamba);

            _logger.LogInformation(@"**********Processo de atualizacao de tipo cacamba concluido com sucesso.**********");

            return new("Tipo Caçamba atualizado", true);
        }
    }
}
