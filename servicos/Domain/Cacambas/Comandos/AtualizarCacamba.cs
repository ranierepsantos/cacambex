using Domain.Cacambas.Enumeraveis;
using Domain.Cacambas.Interface;
using Domain.Cacambas.Validacoes;
using Domain.Compartilhado;
using Domain.Omie.Cacambas.OmieCacambaRequest;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Cacambas.Comandos;

public record AtualizarCacambaComando(
    int Id,
    string Numero,
    string Volume,
    decimal Preco,
    Status Status) : IRequest<Resposta>;
public class AtualizarCacamba : IRequestHandler<AtualizarCacambaComando, Resposta>
{
    private readonly ICacambaRepositorio _repositorio;
    private readonly IMediator _mediator;
    private readonly ILogger<AtualizarCacamba> _logger;
    public AtualizarCacamba(ICacambaRepositorio repositorio, IMediator mediator, ILogger<AtualizarCacamba> logger)
    {
        _repositorio = repositorio;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Resposta> Handle(AtualizarCacambaComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(@"
        **********Processo de atualizacao de cacamba iniciado.**********");

        #region validacoes
        if (request is null)
        {
            _logger.LogError(@"
        **********Request não pode ser nulo.**********");
            return new("Request não pode ser nulo.", false);
        }
        var validacao = new AtualizarCacambaValidacao();
        var resultadoValidacao = validacao.Validate(request);
        if (!resultadoValidacao.IsValid)
        {
            _logger.LogError(@"
        **********Processo de atualizacao de caçamba falhou devido a erros de validação.**********");
            return new("", false, resultadoValidacao.Errors.Select(x => x.ErrorMessage));

        }

        var cacamba = _repositorio.ObterPorId(request.Id);
        if (cacamba is null)
        {
            _logger.LogError(@"
        **********Caçamba não encontrada.**********");
            return new("Caçamba não encontrada.", false);
        }
        #endregion

        #region omie
        IntEditar intEditar = new(cacamba.cCodIntServ, cacamba.nCodServ);
        CabecalhoAtualizar cabecalho = new(request.Numero, request.Preco);
        OmieAlterarCacambaRequest cacambaRequest = new(intEditar, cabecalho);

        var omieResponse = await _mediator.Send(cacambaRequest);
        if (!omieResponse.Sucesso)
        {
            _logger.LogError(@"
            **********AtualizarCacamba: ocorreu um erro na requisicao para a Omie. Erro: {0}**********", new { omieResponse });
            return omieResponse;
        }
        #endregion

        #region local
        cacamba.AtualizarCacamba(request.Numero, request.Volume, request.Preco);
        await _repositorio.AtualizarCacamba(cacamba);
        #endregion

        _logger.LogInformation(@"
        **********Processo de atualizacao de cacamba concluido com sucesso.**********");
        return omieResponse;
    }
}
