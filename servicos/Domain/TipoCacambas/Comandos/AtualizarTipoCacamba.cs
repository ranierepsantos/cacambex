using Domain.Cacambas.Validacoes;
using Domain.Compartilhado;
using Domain.TipoCacambas.Agregacao;
using Domain.TipoCacambas.Interface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.TipoCacambas.Comandos
{

    public record AlterarPrecoFaixaCep(
    int Id,
    string CepInicial,
    string CepFinal,
    decimal Preco);

    public record AtualizarTipoCacambaComando(
        int Id,
        string Volume,
        decimal Preco,
        Boolean Ativo,
        IList<AlterarPrecoFaixaCep> PrecoFaixaCep) : IRequest<Resposta>;
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

            var tipoCacamba = await _repositorio.ObterTipoCacambaPorIdComPrecoFaixaCepAsync(request.Id);
            if (tipoCacamba is null)
            {
                _logger.LogError(@"********** Tipo Caçamba não encontrada.**********");
                return new("Tipo Caçamba não encontrada.", false);
            }
            #endregion

            //remover preco x faixa x cep que foram removidos
            List<PrecoFaixaCep> removerFaixaPreco = tipoCacamba.PrecoFaixaCep
                            .Where(c => !request.PrecoFaixaCep.Any(r => r.Id == c.Id))
                            .Where(c => c.Id != 0)
                            .ToList();
            
            foreach (var remove in removerFaixaPreco) 
                tipoCacamba.RemoverPrecoFaixaCep(remove);

            // alterar preco x faixa x cep
            List<PrecoFaixaCep> alterarFaixaPreco = tipoCacamba.PrecoFaixaCep
                            .Where(c => request.PrecoFaixaCep.Any(r => r.Id == c.Id))
                            .ToList();

            foreach (var altera in alterarFaixaPreco) { 
                var data = request.PrecoFaixaCep.Where(c=> c.Id == altera.Id).FirstOrDefault();
                if (data != null)
                {
                    altera.Alterar(data.CepInicial, data.CepFinal, data.Preco);
                    tipoCacamba.AlterarPrecoFaixaCep(altera);
                }
            }

            // adicionar preco x faixa x cep
            List<AlterarPrecoFaixaCep> adicionarFaixaPreco = request.PrecoFaixaCep.Where(c => c.Id <= 0).ToList();

            foreach (var adiciona in adicionarFaixaPreco)
            {
                tipoCacamba.AdicionarPrecoFaixaCep(new PrecoFaixaCep(tipoCacamba.Id, adiciona.CepInicial, adiciona.CepFinal, adiciona.Preco));
            }
                

            tipoCacamba.AtualizarTipoCacamba(request.Volume, request.Preco, request.Ativo);

            await _repositorio.AtualizarTipoCacambaAsync(tipoCacamba);

            _logger.LogInformation(@"**********Processo de atualizacao de tipo cacamba concluido com sucesso.**********");

            return new("Tipo Caçamba atualizado", true);
        }
    }
}
