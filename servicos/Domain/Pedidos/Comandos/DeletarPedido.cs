using Domain.Compartilhado;
using Domain.Pedidos.Interface;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Pedidos.Comandos;

public record DeletarPedidoComando(int PedidoId) : IRequest<Resposta>;
public class DeletarPedidoValidacao : AbstractValidator<DeletarPedidoComando>
{
    public DeletarPedidoValidacao()
    {
        RuleFor(x => x.PedidoId).NotNull().WithMessage("Pedido inválido");
    }
}
public class DeletarPedido : IRequestHandler<DeletarPedidoComando, Resposta>
{
    private readonly IPedidoRepositorio _repositorio;
    private readonly ILogger<DeletarPedido> _logger;

    public DeletarPedido(IPedidoRepositorio repositorio, ILogger<DeletarPedido> logger)
    {
        _repositorio = repositorio;
        _logger = logger;
    }

    public async Task<Resposta> Handle(DeletarPedidoComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(@"
        **********Processo de exclusao da pedido iniciado**********");

        #region validacoes
        if (request is null)
        {
            _logger.LogError(@"
        **********Request nao pode ser nulo.**********");
            return new("Request nao pode ser nulo.", false);
        }

        var pedido = await _repositorio.ObterPedidoPorIdAsync(request.PedidoId);
        if (pedido is null)
        {
            _logger.LogError(@"
        **********Pedido nao encontrado.**********");
            return new("Pedido nao encontrado.", false);

        }
        #endregion
        pedido.ExcluirPedido(pedido);
        await _repositorio.DeletarPedidoAsync(pedido);

        _logger.LogInformation(@"
        **********Processo de delecao de pedido concluido com sucesso**********");
        return new Resposta("Pedido deletado.");
    }
}
