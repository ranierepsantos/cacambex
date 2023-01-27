using Domain.Pedidos.Enumeraveis;

namespace Domain.Pedidos.Eventos
{
    public class CTR : Evento
    {
        public CTR(StatusPedido status) : base(nameof(CTR), status) { }
    }
}