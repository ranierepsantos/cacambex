using System.Linq.Expressions;
using Domain.Clientes.Agrecacao;
using Domain.Pedidos.Agregacao;
using Domain.Pedidos.Enumeraveis;

namespace Domain.Pedidos.Visualizacoes
{
    public class VisualizarPedido
    {
        public int Id { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public int IdCliente { get; set; }
        public string DocumentoCliente { get; set; } = string.Empty;
        public string TelefoneCliente { get; set; } = string.Empty;
        public string EmailCliente { get; set; } = string.Empty;
        public string? NumeroNotaFiscal { get; set; } = string.Empty;
        public string? NumeroCTR { get; set; } = string.Empty;
        public TipoDePagamento TipoDePagamento { get; set; }
        public PedidoItem PedidoItem { get; set; } = null!;
        public EnderecoEntrega EnderecoEntrega { get; set; } = null!;
        public decimal ValorPedido { get; set; }
        public string? Observacao { get; set; } = string.Empty;
        public List<VisualizarEvento> Eventos { get; set; } = null!;
        public DateTime EmitidoEm { get; set; }
    }

    public static class VisualizarPedidoExtensao
    {
        public static Expression<Func<Pedido, VisualizarPedido>> ToView() => x => new VisualizarPedido
        {
            Id = x.Id,
            NomeCliente = x.Cliente.Nome,
            IdCliente = x.Cliente.Id,
            DocumentoCliente = x.Cliente.Documento,
            TelefoneCliente = x.Cliente.Telefone,
            EmailCliente = x.Cliente.Email,
            TipoDePagamento = x.TipoDePagamento,
            PedidoItem = x.PedidoItem,
            ValorPedido = x.ValorPedido,
            Observacao = x.Observacao,
            NumeroNotaFiscal = x.NumeroNotaFiscal,
            NumeroCTR = x.NumeroCTR,
            EnderecoEntrega = x.EnderecoEntrega,
            Eventos = new()
            {
                x.PedidoEmitido.ToView(),
                x.PedidoItem.ItemEntregue.ToView(),
                x.NotaFiscal.ToView(),
                x.PedidoItem.CTR.ToView(),
                x.PedidoItem.RecolherItem.ToView(),
                x.PedidoItem.PedidoConcluido.ToView()
            },
        };
    }
}