using Domain.ColetasOnline;

namespace Domain.AzureStorage;

public interface IFilaRetirarCacambaRepositorio
{
    Task FilaRetiraCacamba(RetirarCacambaRequest retirarCacamba);
}
