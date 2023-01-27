using Domain.Autorizacao.Validacoes;
using Domain.Compartilhado;
using Domain.Identidade.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Autorizacao.Comandos;
public record AlterarSenhaRequest(string NovaSenha, string ConfirmarNovaSenha);
public record AlterarSenhaComando(string NovaSenha, string ConfirmarNovaSenha, string Email) : IRequest<Resposta>;
public class AlterarSenhaHandler : IRequestHandler<AlterarSenhaComando, Resposta>
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    private readonly ILogger<AlterarSenhaHandler> _logger;
    public AlterarSenhaHandler(IUsuarioRepositorio usuarioRepositorio, ILogger<AlterarSenhaHandler> logger)
    {
        _usuarioRepositorio = usuarioRepositorio;
        _logger = logger;
    }

    public async Task<Resposta> Handle(AlterarSenhaComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(@"
        **********Processo para alterar senha iniciado**********");
        if (request is null)
        {
            _logger.LogError(@"
        **********Request não pode ser nulo.**********");
            return new("Request não pode ser nulo.", false);
        }

        var validacao = new AlterarSenhaValidacao();
        var resultadoValidacao = validacao.Validate(request);
        if (!resultadoValidacao.IsValid)
        {
            _logger.LogError(@"
        **********Processo para alterar senha falhou devido a erros de validação.**********");
            return new("", false, resultadoValidacao.Errors.Select(x => x.ErrorMessage));
        }

        var usuario = await _usuarioRepositorio.ObterPorEmail(request.Email);
        if (usuario is null)
        {
            _logger.LogError(@"
        **********Não existe um usuário com esse email.**********");
            return new("Não existe um usuário com esse email!", false);
        }

        usuario.AlterarSenha(request.NovaSenha);
        await _usuarioRepositorio.AtualizarUsuario(usuario);
        _logger.LogInformation(@"
        **********Processo para alterar senha concluído com sucesso**********");
        return new("Senha alterada com sucesso!");
    }
}