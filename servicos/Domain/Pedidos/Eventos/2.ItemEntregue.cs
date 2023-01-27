using Domain.Pedidos.Enumeraveis;

namespace Domain.Pedidos.Eventos
{
    public class Entregue : Evento
    {
        public Entregue(StatusPedido status) : base(nameof(Entregue), status)
        {
        }
    }
}