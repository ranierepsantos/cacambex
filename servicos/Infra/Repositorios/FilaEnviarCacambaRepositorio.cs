using System.Text.Json;
using Azure.Storage.Queues;
using Domain.AzureStorage;
using Domain.ColetasOnline;
using Infra.Dados;

namespace Infra.Repositorios;
public class FilaEnviarCacambaRepositorio : IFilaEnviarCacambaRepositorio
{
    private readonly QueueClient _queueClient;

    public FilaEnviarCacambaRepositorio(StorageContext _storageContext)
    {
        _queueClient = new(_storageContext.ConnectionString, _storageContext.FilaEnviarCacamba, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });
    }

    public async Task FilaEnviarCacamba(EnviarCacambaRequest enviarCacamba)
    {
        _queueClient.CreateIfNotExists();
        var enviarCacambaString = JsonSerializer.Serialize(enviarCacamba);
        if (_queueClient.Exists())
        {
            try
            {
                await _queueClient.SendMessageAsync(enviarCacambaString);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
