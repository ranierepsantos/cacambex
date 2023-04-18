using Domain.Autorizacao.Interfaces;
using Domain.Autorizacao.Validacoes;
using Domain.Compartilhado;
using Domain.Identidade.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Autorizacao.Comandos;
public record EsqueciSenhaRequest(string Email);
public record EsqueciSenhaComando(string Email, string Origin) : IRequest<Resposta>;
public class EsqueciSenhaHandler : IRequestHandler<EsqueciSenhaComando, Resposta>
{
    private readonly ITokenRepositorio _jwtRepositorio;
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    private readonly IEmailRepositorio _emailRepositorio;
    private readonly ILogger<EsqueciSenhaHandler> _logger;
    public EsqueciSenhaHandler(IUsuarioRepositorio usuarioRepositorio, ILogger<EsqueciSenhaHandler> logger, IEmailRepositorio emailRepositorio, ITokenRepositorio jwtRepositorio)
    {
        _usuarioRepositorio = usuarioRepositorio;
        _logger = logger;
        _emailRepositorio = emailRepositorio;
        _jwtRepositorio = jwtRepositorio;
    }

    public async Task<Resposta> Handle(EsqueciSenhaComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(@"
        **********Processo para gerar nova senha iniciado**********");
        if (request is null)
        {
            _logger.LogError(@"
        **********Request não pode ser nulo.**********");
            return new("Request não pode ser nulo.", false);
        }

        var validacao = new EsqueciSenhaValidacao();
        var resultadoValidacao = validacao.Validate(request);
        if (!resultadoValidacao.IsValid)
        {
            _logger.LogError(@"
        **********Processo de envio de e-mail para alteração de senha falhou devido a erros de validação.**********");
            return new("", false, resultadoValidacao.Errors.Select(x => x.ErrorMessage));
        }

        var usuario = await _usuarioRepositorio.ObterPorEmail(request.Email);
        if (usuario is null)
        {
            _logger.LogError(@"
        **********Não existe um usuário cadastrado com esse email.**********");
            return new("Não existe um usuário cadastrado com esse email!", false);
        }
        string token = _jwtRepositorio.GerarToken(usuario, usuario.Id);
        await _emailRepositorio.EnviarEmailRecuperarSenha(usuario, token, request.Origin);
        return new("Um link para cadastro de uma nova senha foi enviado para o seu e-mail.");
    }
}