using Domain.Cacambas.Comandos;
using Domain.Cacambas.Consultas;
using Domain.Cacambas.Enumeraveis;
using Domain.Cacambas.Visualizacoes;
using Domain.Compartilhado;
using Infra.Dados;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("cacamba")]

public class CacambaController : ControllerBase
{
    private readonly DataContext _db;
    private readonly IMediator _mediator;

    public CacambaController(DataContext db, IMediator mediator)
    {
        _db = db;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> Criar(NovaCacambaComando comando)
    {
        var result = await _mediator.Send(comando);
        if (!result.Sucesso)
            return BadRequest(result);
        return Created("", result);
    }

    [HttpGet("cacambas-disponiveis")]
    public ActionResult<List<VisualizarCacamba>> ObterCacambasDisponiveis()
    {
        return _db.Cacambas
        .Where(x => x.Status == Status.Disponivel && x.Ativo == true)
        .Select(x => new VisualizarCacamba
        {
            Id = x.Id,
            Numero = x.Numero,
            Preco = x.Preco,
            Volume = x.Volume,
            Status = x.Status,

        }).ToList();
    }
    [HttpGet]
    public ActionResult<Paginacao<VisualizarCacamba>> Get(
    [FromQuery] ConsultarCacamba consultar)
    {
        var query = _db.Cacambas
        .AsSingleQuery()
        .Where(x => x.Ativo == true)
        .OrderBy(x => x.Id)
        .Select(VisualizarCacambaExtensao.ToView());

        if (consultar.Sort == "desc")
            query = query.OrderByDescending(x => x.Id);
        else
            query = query.OrderBy(x => x.Id);

        return new Paginacao<VisualizarCacamba>(query, consultar.PageIndex, consultar.PageSize);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Atualizar(int id, AtualizarCacambaComando comando)
    {
        if (id != comando.Id)
            return BadRequest(new Resposta("ID da rota é diferente do ID da caçamba", false, new { idDaRota = id, comando }));
        var result = await _mediator.Send(comando);
        if (!result.Sucesso)
            return BadRequest(result);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Deletar(int id)
    {
        var req = new DeletarCacambaComando(id); ;
        var result = await _mediator.Send(req);
        if (!result.Sucesso)
            return BadRequest(result);
        return NoContent();
    }
}