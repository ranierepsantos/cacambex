using Domain.Identidade.Comandos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("auto-cadastro")]
public class AutoCadastroController : Controller
{
    private readonly IMediator _mediator;
    public AutoCadastroController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> AutoCadastrar(NovoAutoCadastroComando comando)
    {
        var result = await _mediator.Send(comando);
        if (!result.Sucesso)
            return BadRequest(result);
        return Ok(result);
    }
}
