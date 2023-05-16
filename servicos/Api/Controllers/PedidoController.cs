using Domain.Compartilhado;
using Domain.Pedidos.Comandos;
using Domain.Pedidos.Consultas;
using Domain.Pedidos.Visualizacoes;
using Infra.Dados;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

//[Authorize]
[ApiController]
[Route("pedido")]
public class PedidoController : Controller
{
    private readonly IMediator _mediator;
    private readonly IPedidoConsulta _consulta;
    private readonly DataContext _db;
    public PedidoController(IMediator mediator, IPedidoConsulta consulta, DataContext db)
    {
        _mediator = mediator;
        _consulta = consulta;
        _db = db;
    }

    [HttpPost]
    public async Task<ActionResult> Criar(NovoPedidoComando novoPedido)
    {
        var result = await _mediator.Send(novoPedido);
        if (!result.Sucesso)
            return BadRequest(result);
        return Created("", result);
    }

    [HttpGet("{id}")]
    public ActionResult ObterPedidoPorId(int id) => Ok(_consulta.ObterPorId(id));

    [HttpGet("obter-pedidos-cliente/{clienteId}")]
    public ActionResult<Paginacao<VisualizarPedido>> ObterPedidosPorClienteId([FromQuery] ConsultaPedidos consulta, int clienteId)
    {
        var query = _db.Pedidos
            .Where(p => p.Ativo == true && p.Cliente.Id == clienteId)
            .Include(p => p.PedidoEmitido)
            .Include(p => p.Cliente)
            .Include(p => p.PedidoItem)
            .ThenInclude(p => p.Cacamba)
            .Include(p => p.EnderecoEntrega)
            .Select(VisualizarPedidoExtensao.ToView())
            ;

        if (consulta.Sort == "desc")
            query = query.OrderByDescending(x => x.Id);
        else
            query = query.OrderBy(x => x.Id);

        if (!string.IsNullOrEmpty(consulta.DocumentoCliente))
            query = query.Where(x => x.DocumentoCliente == consulta.DocumentoCliente);

        if (!string.IsNullOrEmpty(consulta.NomeCliente))
            query = query.Where(x => x.NomeCliente.ToUpper().Contains(consulta.NomeCliente.ToUpper()));

        //IMPLANTAR
        // if (consulta.FiltrarPorData)
        // {
        //     var inicio = (DateTime)consulta.DataInicio;
        //     var fim = (DateTime)consulta.DataFim;
        //     query = query.Where(x => x.EmitidoEm >= inicio.Date);
        //     query = query.Where(x => x.EmitidoEm <= fim.Date);
        // }
        return new Paginacao<VisualizarPedido>(query, consulta.PageIndex, consulta.PageSize);
    }
    [HttpGet]
    public ActionResult<Paginacao<VisualizarPedido>> Get([FromQuery] ConsultaPedidos consulta)
    {
        var query = _db.Pedidos
            .Where(p => p.Ativo == true)
            .Include(p => p.PedidoEmitido)
            .Include(p => p.Cliente)
            .Include(p => p.PedidoItem)
            .ThenInclude(p => p.Cacamba)
            .Include(p => p.EnderecoEntrega)
            .Select(VisualizarPedidoExtensao.ToView());

        if (consulta.Sort == "desc")
            query = query.OrderByDescending(x => x.Id);
        else
            query = query.OrderBy(x => x.Id);

        if (!string.IsNullOrEmpty(consulta.DocumentoCliente))
            query = query.Where(x => x.DocumentoCliente == consulta.DocumentoCliente);

        if (!string.IsNullOrEmpty(consulta.NomeCliente))
            query = query.Where(x => x.NomeCliente.ToUpper().Contains(consulta.NomeCliente.ToUpper()));

        if (!string.IsNullOrEmpty(consulta.NotaFiscal))
            query = query.Where(x => x.NumeroNotaFiscal.Equals(consulta.NotaFiscal));
        
        if (!string.IsNullOrEmpty(consulta.NumeroCTR))
            query = query.Where(x => x.NumeroCTR.Equals(consulta.NumeroCTR));


        //IMPLANTAR
        // if (consulta.FiltrarPorData)
        // {
        //     var inicio = (DateTime)consulta.DataInicio;
        //     var fim = (DateTime)consulta.DataFim;
        //     query = query.Where(x => x.EmitidoEm >= inicio.Date);
        //     query = query.Where(x => x.EmitidoEm <= fim.Date);
        // }
        return new Paginacao<VisualizarPedido>(query, consulta.PageIndex, consulta.PageSize);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Atualizar(int id, AtualizarPedidoComando requisicao)
    {
        if (id != requisicao.PedidoId)
            return BadRequest(new Resposta("Id da rota diferente da requisição", false, new { idDaRota = id, requisicao }));
        var result = await _mediator.Send(requisicao);
        if (!result.Sucesso)
            return BadRequest(result);
        return NoContent();
    }

    [HttpPut("emitir-nota-fiscal/{pedidoId}")]
    public async Task<ActionResult> EmitirNotaFiscal(int pedidoId, EmitirNotaFiscalComando requisicao)
    {
        if (pedidoId != requisicao.PedidoId)
            return BadRequest(new Resposta("Id da rota diferente da requisição", false, new { idDaRota = pedidoId, requisicao }));
        var result = await _mediator.Send(requisicao);
        if (!result.Sucesso)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("consultar-status-nota-fiscal/{pedidoId}")]
    public async Task<ActionResult> ConsultarStatusNotaFiscal(int pedidoId, ConsultarStatusNotaFiscalQuery requisicao)
    {
        if (pedidoId != requisicao.PedidoId)
            return BadRequest(new Resposta("Id da rota diferente da requisição", false, new { idDaRota = pedidoId, requisicao }));
        var result = await _mediator.Send(requisicao);
        if (!result.Sucesso)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("emitir-ctr/{pedidoId}")]
    public async Task<ActionResult> EmitirCtr(int pedidoId, SolicitaCtrComando requisicao)
    {
        if (pedidoId != requisicao.PedidoId)
            return BadRequest(new Resposta("Id da rota diferente da requisição", false, new { idDaRota = pedidoId, requisicao }));
        var result = await _mediator.Send(requisicao);
        if (!result.Sucesso)
            return BadRequest(result);

        return NoContent();
    }

    [HttpPut("enviar-cacamba/{pedidoId}/")]
    public async Task<ActionResult> EnviarCacamba(int pedidoId, EnviarCacambaComando requisicao)
    {
        if (pedidoId != requisicao.PedidoId)
            return BadRequest(new Resposta("Id da rota diferente da requisição", false, new { idDaRota = pedidoId, requisicao }));
        var result = await _mediator.Send(requisicao);
        if (!result.Sucesso)
            return BadRequest(result);

        return NoContent();
    }

    [HttpPut("retirar-cacamba/{pedidoId}/")]
    public async Task<ActionResult> RetirarCacamba(int pedidoId, RetirarCacambaComando requisicao)
    {
        if (pedidoId != requisicao.PedidoId)
            return BadRequest(new Resposta("Id da rota diferente da requisição", false, new { idDaRota = pedidoId, requisicao }));
        var result = await _mediator.Send(requisicao);
        if (!result.Sucesso)
            return BadRequest(result);

        return NoContent();
    }

    [HttpPut("vincular-cacamba/{pedidoId}")]
    public async Task<ActionResult> VincularCacamba(int pedidoId, VincularCacambaComando requisicao)
    {
        if (pedidoId != requisicao.PedidoId)
            return BadRequest(new Resposta("Id da rota diferente da requisição", false, new { idDaRota = pedidoId, requisicao }));
        var result = await _mediator.Send(requisicao);
        if (!result.Sucesso)
            return BadRequest(result);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Deletar(int id)
    {
        var requisicao = new DeletarPedidoComando(id);
        var result = await _mediator.Send(requisicao);
        if (!result.Sucesso)
            return BadRequest(result);
        return NoContent();
    }

    [HttpGet("notificar-recolher/{dias}")]
    public async Task<ActionResult<IList<VisualizarPedido>>> NotificarPedido(int dias)
    {
        string query = "SELECT pe.* FROM pedidos pe (nolock) " +
            "INNER JOIN pedidoitens px (nolock) on px.id = pe.id " +
            "INNER JOIN eventos ee (nolock) on ee.id = px.itementregueid and ee.status = 0 " +
            "INNER JOIN eventos er (nolock) on er.id = px.recolheritemid and er.status = 3 " +
            $"WHERE DATEDIFF(DAY, ee.quando, getdate()) >={dias}";

        var data = await _db.Pedidos.FromSqlRaw(query).OrderBy(x => x.Id).Select(VisualizarPedidoExtensao.ToView()).ToListAsync();

        return data;
    }
}
