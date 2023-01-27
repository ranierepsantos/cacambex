using Domain.Cacambas.Agregacao;
using Domain.Compartilhado;
using Domain.Pedidos.Enumeraveis;
using Domain.Pedidos.Eventos;

namespace Domain.Pedidos.Agregacao;

public class PedidoItem : Entidade, IAggregateRoot
{
    public string VolumeCacamba { get; private set; } = string.Empty;
    public Cacamba? Cacamba { get; private set; } = null;
    public decimal ValorUnitario { get; private set; }
    public CTR CTR { get; private set; } = null!;
    public Recolher RecolherItem { get; private set; } = null!;
    public Entregue ItemEntregue { get; private set; } = null!;
    public Concluido PedidoConcluido { get; private set; } = null!;


    public PedidoItem(string volumeCacamba, decimal valorUnitario)
    {
        VolumeCacamba = volumeCacamba;
        ValorUnitario = valorUnitario;
        CTR = new CTR(StatusPedido.NaoEmitido);
        RecolherItem = new Recolher(StatusPedido.NaoEmitido);
        ItemEntregue = new Entregue(StatusPedido.NaoEmitido);
        PedidoConcluido = new Concluido(StatusPedido.NaoEmitido);
    }
    public void VincularCacamba(Cacamba cacamba)
    {
        ItemEntregue.AtualizarStatus(StatusPedido.Concluido, "Cacamba entregue");
        Cacamba = cacamba;
    }
    public void AtualizarPedidoItem(string volumeCacamba, decimal valorUnitario)
    {
        VolumeCacamba = volumeCacamba;
        ValorUnitario = valorUnitario;
    }
    public void AtualizarCTR(string mensagem)
    {
        CTR.AtualizarStatus(StatusPedido.Aguardando, mensagem);
    }

    public void RecolherCacamba(string mensagem)
    {
        RecolherItem.AtualizarStatus(StatusPedido.Aguardando, mensagem);
        PedidoConcluido.AtualizarStatus(StatusPedido.Aguardando, "");
    }
    protected PedidoItem() { }
}