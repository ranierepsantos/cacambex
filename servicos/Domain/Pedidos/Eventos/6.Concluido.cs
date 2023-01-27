using Domain.Pedidos.Enumeraveis;

namespace Domain.Pedidos.Eventos
{
    public class Concluido : Evento
    {
        public Concluido(StatusPedido status) : base(nameof(Concluido), status) { }
    }
}