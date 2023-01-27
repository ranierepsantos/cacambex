using System.Text.Json;
using Domain.Cacambas.Agregacao;
using Domain.Cacambas.Interface;
using Domain.Cacambas.Validacoes;
using Domain.Compartilhado;
using Domain.Omie.Cacambas.OmieCacambaRequest;
using Domain.Omie.Cacambas.OmieCacambaResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Cacambas.Comandos;

public record NovaCacambaComando(string Numero, string Volume, decimal Preco) : IRequest<Resposta>;
public class NovaCacamba : IRequestHandler<NovaCacambaComando, Resposta>
{
    private readonly ICacambaRepositorio _repositorio;
    private readonly IMediator _mediator;
    private readonly ILogger<NovaCacamba> _logger;

    public NovaCacamba(ICacambaRepositorio repositorio, IMediator mediator, ILogger<NovaCacamba> logger)
    {
        _repositorio = repositorio;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Resposta> Handle(NovaCacambaComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(@"
        **********Processo de criação de cacamba iniciado.**********");

        #region validacoes
        if (request is null)
        {
            _logger.LogError(@"
        **********Request não pode ser nulo.**********");
            return new("Request não pode ser nulo.", false);
        }
        var validacao = new NovaCacambaValidacao();
        var resultadoValidacao = validacao.Validate(request);
        if (!resultadoValidacao.IsValid)
        {
            _logger.LogError(@"
        **********Processo de criacao de caçamba falhou devido a erros de validação.**********");
            return new("", false, resultadoValidacao.Errors.Select(x => x.ErrorMessage));
        }

        var existeCacamba = await _repositorio.CacambaExiste(request.Numero);
        if (existeCacamba)
        {
            _logger.LogError(@"
        **********Já existe uma cacamba cadastrada com esse numero!**********");
            return new("Já existe uma cacamba cadastrada com esse numero!", false);
        }
        #endregion

        #region omie

        IntIncluir intIncluir = new(request.Numero);
        var descricao = "caçamba " + request.Numero + "/" + "volume " + request.Volume;
        string cCodServMun = "7.09.03/723";
        string cCodLC116 = "7.09";
        string cIdTrib = "01";
        Cabecalho cabecalho = new(
            request.Numero,
            descricao,
            request.Preco,
            cCodServMun,
            cCodLC116,
            cIdTrib);
        OmieCriarCacambaRequest cacambaRequest = new(intIncluir, cabecalho);

        var omieResponse = await _mediator.Send(cacambaRequest);

        if (!omieResponse.Sucesso)
        {
            _logger.LogError(@"
            **********NovaCacamba: ocorreu um erro na requisicao para a Omie. Erro: {0}**********", new { omieResponse });
            return omieResponse;
        }
        #endregion

        #region local

        List<OmieCriarServicoResult> dados = new();
        dados.Add(omieResponse.Dados as OmieCriarServicoResult);

        string cCodIntServ = "0";
        long nCodServ = 1;

        foreach (var dado in dados)
        {
            nCodServ = dado.nCodServ;
            cCodIntServ = dado.cCodIntServ;
        }
        var cacamba = new Cacamba(request.Numero, request.Volume, request.Preco);
        cacamba.AtualizarCodigosCacambaOmie(nCodServ, cCodIntServ);
        await _repositorio.IncluirCacamba(cacamba);

        #endregion

        _logger.LogInformation(@"
        **********Processo de criacao de cacamba concluido com sucesso.**********");
        return omieResponse;
    }
}
