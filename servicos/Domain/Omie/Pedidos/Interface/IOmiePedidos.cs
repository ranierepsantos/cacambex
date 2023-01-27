using Domain.Compartilhado;

namespace Domain.Omie.Pedidos.Interface;

public interface IOmiePedidos
{
    Task<Resposta> CriarPedido(OmieRequest request);
    Task<Resposta> AtualizarPedido(OmieRequest request);
    Task<Resposta> FaturarPedido(OmieRequest request);
    Task<Resposta> ConsultarStatusPedido(OmieRequest request);
}
