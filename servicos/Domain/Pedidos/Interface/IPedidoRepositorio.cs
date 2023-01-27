using Domain.Pedidos.Agregacao;

namespace Domain.Pedidos.Interface
{
    public interface IPedidoRepositorio
    {
        Task IncluirPedidoAsync(Pedido pedido);
        Task AtualizarPedidoAsync(Pedido pedido);
        Task DeletarPedidoAsync(Pedido pedido);
        Task<Pedido?> ObterPedidoPorIdAsync(int id);
    }
}