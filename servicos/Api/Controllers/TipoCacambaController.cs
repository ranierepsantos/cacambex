using Domain.Cacambas.Comandos;
using Domain.Cacambas.Consultas;
using Domain.Cacambas.Enumeraveis;
using Domain.Cacambas.Visualizacoes;
using Domain.Compartilhado;
using Domain.TipoCacambas.Agregacao;
using Domain.TipoCacambas.Comandos;
using Domain.TipoCacambas.Consultas;
using Infra.Dados;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

//[Authorize]
[ApiController]
[Route("tipo-cacamba")]

public class TipoCacambaController : ControllerBase
{
    private readonly IMediator _mediator;

    public TipoCacambaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<Paginacao<VisualizarCacamba>>> Get(
    [FromQuery] PaginarTipoCacamba consulta)
    {
        Resposta result = await _mediator.Send(consulta);

        return Ok(result.Dados);


    }


    [HttpGet("listar-com-preco-faixa-cep")]
    public async Task<ActionResult<IList<TipoCacamba>>> Get(
    [FromQuery] ListarComPrecoFaixaCepComando consulta)
    {
        Resposta result = await _mediator.Send(consulta);

        return Ok(result.Dados);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Atualizar(int id, AtualizarTipoCacambaComando comando)
    {
        if (id != comando.Id)
            return BadRequest(new Resposta("ID da rota é diferente do ID do Tipo da Caçamba", false, new { idDaRota = id, comando }));
        var result = await _mediator.Send(comando);
        if (!result.Sucesso)
            return BadRequest(result);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> ObterPorId(int id)
    {
        var comando = new ConsultarPorIdComando(id);
        var result = await _mediator.Send(comando);
        if (!result.Sucesso)
            return BadRequest(result);
        return Ok(result.Dados);
    }

    //ConsultarPorIdComando
}