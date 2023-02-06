using Domain.Autorizacao;
using Domain.Autorizacao.Comandos;
using Infra.Dados;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("autenticacao")]
public class AutenticacaoController : ControllerBase
{
    IConfiguration _configuration;
    DataContext _context;
    private readonly IMediator _mediator;

    public AutenticacaoController(IConfiguration configuration, DataContext context, IMediator mediator)
    {
        _configuration = configuration;
        _context = context;
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> Autorizacao(AutenticacaoUsuarioComando comando)
    {
        var response = await _mediator.Send(comando);
        if (!response.Sucesso)
            return BadRequest(response);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("recuperar-senha")]
    public async Task<ActionResult> RecuperarSenha(EsqueciSenhaRequest request)
    {
        var origin = Request.Headers["origin"];
        EsqueciSenhaComando comando = new(request.Email, origin);

        var response = await _mediator.Send(comando);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult> AlterarSenha(AlterarSenhaRequest request)
    {
        var usuarioEmail = User.Identities.First().Claims.First().Value;
        AlterarSenhaComando comando = new(request.NovaSenha, request.ConfirmarNovaSenha, usuarioEmail);

        var response = await _mediator.Send(comando);
        if (!response.Sucesso) return BadRequest(response);
        return Ok(response);
    }
}
