using Domain.Pedidos.Enumeraveis;

namespace Domain.Pedidos.Eventos
{
    public class Recolher : Evento
    {
        public Recolher(StatusPedido status) : base(nameof(Recolher), status) { }
    }
}