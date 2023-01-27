using Domain.Pedidos.Enumeraveis;

namespace Domain.Pedidos.Eventos
{
    public class PedidoEmitido : Evento
    {
        public PedidoEmitido(StatusPedido status) : base(nameof(PedidoEmitido), status)
        {
        }
    }
}