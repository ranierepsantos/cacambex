using Domain.Autorizacao.Interfaces;
using Domain.Clientes.Interface;
using Domain.Compartilhado;
using Domain.Identidade.Enumeraveis;
using Domain.Identidade.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Autorizacao;
public record AutenticacaoUsuarioComando(string Email, string Senha) : IRequest<Resposta>;
public class AutenticacaoUsuarioHandler : IRequestHandler<AutenticacaoUsuarioComando, Resposta>
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    private readonly IClienteRepositorio _clienteRepositorio;
    private readonly IAutenticacaoRepositorio _autenticacaoRepositorio;
    private readonly ITokenRepositorio _jwtRepositorio;
    private readonly ILogger<AutenticacaoUsuarioHandler> _logger;

    public AutenticacaoUsuarioHandler(IUsuarioRepositorio usuarioRepositorio, IAutenticacaoRepositorio autorizacaoRepositorio, ITokenRepositorio jwtRepositorio, ILogger<AutenticacaoUsuarioHandler> logger, IClienteRepositorio clienteRepositorio)
    {
        _usuarioRepositorio = usuarioRepositorio;
        _autenticacaoRepositorio = autorizacaoRepositorio;
        _jwtRepositorio = jwtRepositorio;
        _logger = logger;
        _clienteRepositorio = clienteRepositorio;
    }

    public async Task<Resposta> Handle(AutenticacaoUsuarioComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(@"
        **********Processo de Autenticacao iniciado**********");
        if (request is null)
        {
            _logger.LogError(@"
        **********Request não pode ser nulo.**********");
            return new("Request não pode ser nulo.", false);
        }

        var existeEmail = await _usuarioRepositorio.ExisteEmail(request.Email);
        if (existeEmail is false)
        {
            _logger.LogError(@"
        **********Não existe um usuário com esse email.**********");
            return new("Não existe um usuário com esse email!", false);
        }
        var usuario = await _autenticacaoRepositorio.Autenticacao(request.Email, request.Senha);
        if (usuario is null)
        {
            _logger.LogError(@"
        **********Senha incorreta.**********");
            return new("Senha incorreta!", false);
        }

        string jwtToken = "";

        switch (usuario.Funcao)
        {
            case Funcao.Administrador:
                jwtToken = _jwtRepositorio.GerarToken(usuario, 1);
                break;

            default:
                var cliente = _clienteRepositorio.ObterPorEmail(usuario.Email);
                jwtToken = _jwtRepositorio.GerarToken(usuario, cliente.Id);
                break;
        }

        _logger.LogInformation(@"
        **********Acesso liberado.**********");
        return new("Acesso liberado.", true, new { Token = jwtToken });
    }
}