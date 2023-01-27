using Domain.Compartilhado;
using Domain.Identidade.Enumeraveis;
using Domain.Identidade.Interfaces;
using Domain.Identidade.Validacoes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Identidade.Comandos;
public record AtualizarUsuarioComando(
    int Id,
    string Nome,
    string Email,
    Funcao Funcao) : IRequest<Resposta>;
public class AtualizarUsuario : IRequestHandler<AtualizarUsuarioComando, Resposta>
{
    private readonly IUsuarioRepositorio _repositorio;
    private readonly ILogger<AtualizarUsuario> _logger;
    public AtualizarUsuario(IUsuarioRepositorio repositorio, ILogger<AtualizarUsuario> logger)
    {
        _repositorio = repositorio;
        _logger = logger;
    }

    public async Task<Resposta> Handle(AtualizarUsuarioComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(@"
        **********Processo de atualização de usuário iniciado.**********");
        if (request is null)
        {
            _logger.LogError(@"
        **********Request não pode ser nulo.**********");
            return new("Request não pode ser nulo.", false);
        }

        var validacao = new AtualizarUsuarioValidacao();
        var resultadoValidacao = validacao.Validate(request);
        if (!resultadoValidacao.IsValid)
        {
            _logger.LogError(@"
        **********Processo de atualização de usuário falhou devido a erros de validação.**********");
            return new("", false, resultadoValidacao.Errors.Select(x => x.ErrorMessage));

        }

        var usuario = _repositorio.ObterPorId(request.Id);
        if (usuario is null)
        {
            _logger.LogError(@"
        **********Usuário inexistente.**********");
            return new("Usuário inexistente.", false);
        }

        usuario.AtualizarUsuario(request.Nome, request.Email, request.Funcao);

        await _repositorio.AtualizarUsuario(usuario);
        _logger.LogInformation(@"
        **********Processo de atualização de usuário concluído com sucesso.**********");
        return new("Usuário atualizado com sucesso.");
    }
}
