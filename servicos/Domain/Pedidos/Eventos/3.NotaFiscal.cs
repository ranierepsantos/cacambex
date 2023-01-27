using Domain.Pedidos.Enumeraveis;

namespace Domain.Pedidos.Eventos
{
    public class NotaFiscal : Evento
    {
        public NotaFiscal(StatusPedido status) : base(nameof(NotaFiscal), status)
        {
        }
    }
}