using Domain.Compartilhado;
using Domain.Identidade.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Identidade.Comandos;
public record DeletarUsuarioComando(int Id) : IRequest<Resposta>;
public class DeletarUsuario : IRequestHandler<DeletarUsuarioComando, Resposta>
{
    private readonly IUsuarioRepositorio _repositorio;
    private readonly ILogger<DeletarUsuario> _logger;

    public DeletarUsuario(IUsuarioRepositorio repositorio, ILogger<DeletarUsuario> logger)
    {
        _repositorio = repositorio;
        _logger = logger;
    }

    public async Task<Resposta> Handle(DeletarUsuarioComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(@"
        **********Processo de exclusão de usuário iniciado.**********");
        if (request is null)
        {
            _logger.LogError(@"
        **********Request não pode ser nulo.**********");
            return new("Request não pode ser nulo.", false);
        }

        var usuario = _repositorio.ObterPorId(request.Id);
        if (usuario is null)
        {
            _logger.LogError(@"
        **********Usuário inexistente.**********");
            return new("Usuário inexistente.", false);
        }

        usuario.ExcluirUsuario(usuario);
        await _repositorio.DeletarUsuario(usuario);

        _logger.LogInformation(@"
        **********Processo de exclusão de usuário concluído com sucesso.**********");
        return new("Usuário deletado com sucesso.");
    }
}
