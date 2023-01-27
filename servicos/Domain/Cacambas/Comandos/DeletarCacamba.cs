using Domain.Cacambas.Interface;
using Domain.Compartilhado;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Cacambas.Comandos;

public record DeletarCacambaComando(int Id) : IRequest<Resposta>;
public class DeletarCacamba : IRequestHandler<DeletarCacambaComando, Resposta>
{
    private readonly ICacambaRepositorio _repositorio;
    private readonly ILogger<DeletarCacamba> _logger;
    public DeletarCacamba(ICacambaRepositorio repositorio, ILogger<DeletarCacamba> logger)
    {
        _repositorio = repositorio;
        _logger = logger;
    }

    public async Task<Resposta> Handle(DeletarCacambaComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(@"
        **********Processo de exclusão da cacamba iniciado**********");

        #region validacoes
        if (request is null)
        {
            _logger.LogError(@"
        **********Request não pode ser nulo.**********");
            return new("Request não pode ser nulo.", false);
        }

        var cacamba = _repositorio.ObterPorId(request.Id);
        if (cacamba is null)
        {
            _logger.LogError(@"
        **********Caçamba não encontrada.**********");
            return new("Caçamba não encontrada.", false);

        }
        #endregion
        cacamba.ExcluirCacamba(cacamba);
        await _repositorio.DeletarCacamba(cacamba);

        _logger.LogInformation(@"
        **********Processo de delecao de cacamba concluido com sucesso**********");
        return new("Caçamba excluída com sucesso.");

    }
}
