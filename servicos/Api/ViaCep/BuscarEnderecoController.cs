using System.Threading.Tasks;
using Domain.ViaCep.Interface;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.ViaCep;
[ApiController]
[Route("buscar-endereco")]
public class BuscarEnderecoController : ControllerBase
{
    private readonly IBuscarCep _buscarCep;
    private readonly IMediator _mediator;


    public BuscarEnderecoController(IBuscarCep buscarCep, IMediator mediator)
    {
        _buscarCep = buscarCep;
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<ActionResult> BuscarEndereco(string cep)
    {
        var endereco = await _buscarCep.BuscarEndereco(cep);
        if (endereco is null) return BadRequest();
        return Ok(endereco);
    }
}