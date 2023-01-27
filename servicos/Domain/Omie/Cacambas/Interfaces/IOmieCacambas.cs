using Domain.Compartilhado;

namespace Domain.Omie.Cacambas.Interfaces;

public interface IOmieCacambas
{
    Task<Resposta> Create(OmieRequest request);
    Task<Resposta> Update(OmieRequest request);

}
