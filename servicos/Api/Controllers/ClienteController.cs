using Domain.Clientes.Comandos;
using Domain.Clientes.Consultas;
using Domain.Clientes.Visualizacoes;
using Domain.Compartilhado;
using Infra.Dados;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("cliente")]
public class ClienteController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IClienteConsulta _consulta;
    private readonly DataContext _db;
    public ClienteController(IClienteConsulta consulta, IMediator mediator, DataContext db)
    {
        _mediator = mediator;
        _consulta = consulta;
        _db = db;
    }

    [HttpPost]
    public async Task<ActionResult> Criar(CriarClienteComando comando)
    {
        var result = await _mediator.Send(comando);
        if (!result.Sucesso)
            return BadRequest(result);
        return Created("", result);
    }

    [HttpPost("novo-endereco-entrega")]
    public async Task<ActionResult> NovoEnderecoEntrega(CriarEnderecoEntregaComClienteIdComando comando)
    {
        var result = await _mediator.Send(comando);
        if (!result.Sucesso)
            return BadRequest(result);
        return Created("", result);
    }

    [HttpGet("{id}")]
    public ActionResult ObterClientePorId(int id) => Ok(_consulta.ObterPorId(id));

    [HttpGet]
    public ActionResult<Paginacao<VisualizarCliente>> Get([FromQuery] ConsultarClientes consultar)
    {
        var query = _db.Clientes
      .Include(x => x.EnderecosEntrega)
      .AsSingleQuery()
      .Where(x => x.Ativo == true)
      .Select(VisualizarClienteExtensao.ToView());

        return new Paginacao<VisualizarCliente>(query, consultar.PageIndex, consultar.PageSize);
    }
    [HttpGet("clientes-excluidos")]
    public ActionResult<Paginacao<VisualizarCliente>> GetClientesExcluidos([FromQuery] ConsultarClientes consultar)
    {
        var query = _db.Clientes
      .Include(x => x.EnderecosEntrega)
      .AsSingleQuery()
      .Where(x => x.Ativo == false)
      .Select(VisualizarClienteExtensao.ToView());

        return new Paginacao<VisualizarCliente>(query, consultar.PageIndex, consultar.PageSize);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Atualizar(int id, AtualizarClienteComando comando)
    {
        if (id != comando.Id)
            return BadRequest(new Resposta("Id da rota diferente da requisição", false, new { idDaRota = id, comando }));
        var result = await _mediator.Send(comando);
        if (!result.Sucesso)
            return BadRequest(result);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Deletar(int id)
    {
        var req = new DeletarClienteComando(id);
        var result = await _mediator.Send(req);
        if (!result.Sucesso) return BadRequest(result);
        return NoContent();
    }
}
