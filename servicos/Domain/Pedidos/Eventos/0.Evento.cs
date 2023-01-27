using Domain.Compartilhado;
using Domain.Pedidos.Enumeraveis;

namespace Domain.Pedidos.Eventos;

public abstract class Evento : Entidade
{
    public DateTime Quando { get; private set; }
    public string Descricao { get; private set; }
    public StatusPedido Status { get; private set; }
    public string Mensagem { get; private set; }

    protected Evento(string descricao, StatusPedido status)
    {
        Descricao = descricao;
        Status = status;
        Quando = DateTime.Now;
        Mensagem = "";
    }

    public void AtualizarStatus(StatusPedido status, string mensagem)
    {
        Status = status;
        Mensagem = mensagem;
        Quando = DateTime.Now;
    }

}
