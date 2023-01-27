using Domain.Clientes.Interface;
using Domain.Compartilhado;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Clientes.Comandos;

public record DeletarClienteComando(int Id) : IRequest<Resposta>;
public class DeletarCliente : IRequestHandler<DeletarClienteComando, Resposta>
{
    private readonly IClienteRepositorio _repositorio;
    private readonly ILogger<DeletarCliente> _logger;
    private readonly IMediator _mediator;
    public DeletarCliente(IClienteRepositorio repositorio, ILogger<DeletarCliente> logger, IMediator mediator)
    {
        _repositorio = repositorio;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<Resposta> Handle(DeletarClienteComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(@"
        **********Iniciando processo de exclusao de cliente.**********");

        var cliente = _repositorio.ObterPorId(request.Id);
        if (cliente is null)
        {
            _logger.LogError(@"
        **********Cliente não encontrado.**********");
            return new("Cliente não encontrado.", false);
        }
        cliente.ExcluirCliente(cliente);
        await _repositorio.DeletarCliente(cliente);
        _logger.LogInformation(@"
        **********Cliente excluído com sucesso.**********");
        return new("Cliente excluído com sucesso.");
    }
}

