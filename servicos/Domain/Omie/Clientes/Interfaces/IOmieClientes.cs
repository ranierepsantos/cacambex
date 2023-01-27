using Domain.Compartilhado;
using Domain.Omie;

namespace Domain.Omie.Clientes.Interfaces;
public interface IOmieClientes
{
    Task<Resposta> OmieCriar(OmieRequest request);
    Task<Resposta> OmieAtualizar(OmieRequest request);
}
