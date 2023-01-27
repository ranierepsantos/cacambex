using Domain.ColetasOnline;

namespace Domain.AzureStorage;

public interface IFilaEnviarCacambaRepositorio
{
    Task FilaEnviarCacamba(EnviarCacambaRequest retirarCacamba);
}