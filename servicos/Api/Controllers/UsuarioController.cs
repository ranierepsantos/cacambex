using Domain.Compartilhado;
using Domain.Identidade.Comandos;
using Domain.Identidade.Consultas;
using Domain.Identidade.Visualizacoes;
using Infra.Dados;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("usuario")]
public class UsuarioController : Controller
{
    private readonly DataContext _db;
    private readonly IMediator _mediator;
    public UsuarioController(DataContext db, IMediator mediator)
    {
        _db = db;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> Criar(CriarUsuarioComando comando)
    {
        var usuario = await _mediator.Send(comando);
        if (!usuario.Sucesso) return BadRequest();
        return Created("", usuario);
    }
    [HttpGet]
    public ActionResult<Paginacao<VisualizarUsuario>> Get(
        [FromQuery] ConsultarUsuario consultar)
    {
        var query = _db.Usuarios
            .AsSingleQuery()
            .Where(x => x.Ativo == true)
            .Select(x => new VisualizarUsuario
            {

                Id = x.Id,
                Nome = x.Nome,
                Email = x.Email,
                Funcao = x.Funcao,
                Ativo = x.Ativo
            });

        if (consultar.Sort == "desc")
            query = query.OrderByDescending(x => x.Id);
        else
            query = query.OrderBy(x => x.Id);
        return new Paginacao<VisualizarUsuario>(query, consultar.PageIndex, consultar.PageSize);
    }
    [HttpGet("usuarios-excluidos")]
    public ActionResult<Paginacao<VisualizarUsuario>> GetUsuariosExcluidos(
        [FromQuery] ConsultarUsuario consultar)
    {
        var query = _db.Usuarios
            .AsSingleQuery()
            .Where(x => x.Ativo == false)
            .Select(x => new VisualizarUsuario
            {

                Id = x.Id,
                Nome = x.Nome,
                Email = x.Email,
                Funcao = x.Funcao,
                Ativo = x.Ativo
            });

        if (consultar.Sort == "desc")
            query = query.OrderByDescending(x => x.Id);
        else
            query = query.OrderBy(x => x.Id);
        return new Paginacao<VisualizarUsuario>(query, consultar.PageIndex, consultar.PageSize);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Atualizar(int id, AtualizarUsuarioComando comando)
    {
        if (id != comando.Id)
            return BadRequest(new Resposta("ID da rota é diferente do ID do usuário", false, new { idDaRota = id, comando }));
        var result = await _mediator.Send(comando);
        if (!result.Sucesso)
            return BadRequest(result);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Deletar(int id)
    {
        var requisicao = new DeletarUsuarioComando(id);
        var result = await _mediator.Send(requisicao);
        if (!result.Sucesso)
            return BadRequest(result);
        return NoContent();
    }
}
