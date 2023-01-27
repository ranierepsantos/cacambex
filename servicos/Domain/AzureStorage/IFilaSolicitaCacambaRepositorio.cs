using Domain.ColetasOnline;

namespace Domain.AzureStorage;

public interface IFilaSolicitaCacambaRepositorio
{
    Task FilaSolicitaCTR(SolicitaCTRRequest ctr);
}
