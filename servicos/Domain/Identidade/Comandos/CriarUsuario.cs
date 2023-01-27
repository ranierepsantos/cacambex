using Domain.Compartilhado;
using Domain.Identidade.Agregacao;
using Domain.Identidade.Enumeraveis;
using Domain.Identidade.Interfaces;
using Domain.Identidade.Validacoes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Identidade.Comandos;
public record CriarUsuarioComando(
    string Nome,
    string Email,
    Funcao Funcao) : IRequest<Resposta>;
public class CriarUsuarioHandler : IRequestHandler<CriarUsuarioComando, Resposta>
{
    private readonly IUsuarioRepositorio _repositorio;
    private readonly ILogger<CriarUsuarioHandler> _logger;
    public CriarUsuarioHandler(IUsuarioRepositorio repositorio, ILogger<CriarUsuarioHandler> logger)
    {
        _repositorio = repositorio;
        _logger = logger;
    }

    public async Task<Resposta> Handle(CriarUsuarioComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(@"
        **********Processo de criação de usuário iniciado.**********");
        if (request is null)
        {
            _logger.LogError(@"
        **********Request não pode ser nulo.**********");
            return new("Request não pode ser nulo.", false);
        }

        var validacao = new CriarUsuarioValidacao();
        var resultadoValidacao = validacao.Validate(request);
        if (!resultadoValidacao.IsValid)
        {
            _logger.LogError(@"
        **********Processo de criação de usuário falhou devido a erros de validação.**********");
            return new("", false, resultadoValidacao.Errors.Select(x => x.ErrorMessage));

        }

        var existeUsuario = await _repositorio.ExisteEmail(request.Email);
        if (existeUsuario)
        {
            _logger.LogError(@"
        **********Usuario já cadastrado com esse e-mail.**********");
            return new("Usuario já cadastrado com esse e-mail.", false);
        }

        var usuario = new Usuario(request.Nome, request.Email, request.Funcao);
        await _repositorio.IncluirUsuario(usuario);
        _logger.LogInformation(@"
        **********Processo de criação de usuário concluído com sucesso.**********");
        return new("Usuario criado com sucesso.");
    }
}