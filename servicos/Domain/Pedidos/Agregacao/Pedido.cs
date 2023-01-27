using Domain.Clientes.Agrecacao;
using Domain.Compartilhado;
using Domain.Pedidos.Enumeraveis;
using Domain.Pedidos.Eventos;
using Domain.Pedidos.Execoes;

namespace Domain.Pedidos.Agregacao;

public class Pedido : Entidade, IAggregateRoot
{
    protected Pedido() { }
    public Pedido(
        Cliente cliente,
        PedidoItem pedidoItem,
        EnderecoEntrega enderecoEntrega,
        TipoDePagamento tipoDePagamento,
        string observacao,
        decimal valorPedido,
        Guid cCodIntOS,
        long nCodOS)
    {
        Cliente = cliente;
        TipoDePagamento = tipoDePagamento;
        PedidoItem = pedidoItem;
        Observacao = observacao;
        EnderecoEntrega = enderecoEntrega;
        ValorPedido = valorPedido;
        Ativo = true;
        PedidoEmitido = new PedidoEmitido(StatusPedido.Concluido);
        NotaFiscal = new NotaFiscal(StatusPedido.NaoEmitido);
        this.cCodIntOS = cCodIntOS;
        this.nCodOS = nCodOS;
    }
    public Cliente Cliente { get; private set; } = null!;
    public Guid cCodIntOS { get; private set; }
    public long nCodOS { get; set; }
    public string? NumeroNotaFiscal { get; private set; } = string.Empty;
    public string? NumeroCTR { get; private set; } = string.Empty;
    public string? Observacao { get; private set; } = string.Empty;
    public PedidoEmitido PedidoEmitido { get; private set; } = null!;
    public TipoDePagamento TipoDePagamento { get; private set; }
    public EnderecoEntrega EnderecoEntrega { get; private set; } = null!;
    public PedidoItem PedidoItem { get; private set; } = null!;
    public decimal ValorPedido { get; private set; }
    public bool Ativo { get; private set; }
    public NotaFiscal NotaFiscal { get; private set; } = null!;
    public void AtibuirNumeroNotaFiscal(string numeroNotaFiscal) => NumeroNotaFiscal = numeroNotaFiscal;
    public void AtualizarPedido(
        string? observacao,
        TipoDePagamento tipoDePagamento,
        EnderecoEntrega enderecoEntrega,
        decimal valorPedido)
    {
        TipoDePagamento = tipoDePagamento;
        Observacao = observacao;
        EnderecoEntrega = enderecoEntrega;
        ValorPedido = valorPedido;
    }
    public void AtualizarNotaFiscal(StatusPedido status, string mensagem)
    {
        NotaFiscal.AtualizarStatus(status, mensagem);
    }
    public void ExcluirPedido(Pedido pedido)
    {
        if (pedido.Ativo == false)
            throw new PedidoExcecoes(nameof(pedido.Ativo), "Pedido já está inativo. Informe um Pedido ativo.");

        pedido.PedidoItem.Cacamba?.AlterarStatus(pedido.PedidoItem.Cacamba);
        pedido.Ativo = false;
    }
}