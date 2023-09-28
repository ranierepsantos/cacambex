using System.Text.Json;
using Azure.Storage.Queues;
using Domain.AzureStorage;
using Domain.ColetasOnline;
using Infra.Dados;
using Microsoft.Extensions.Options;

namespace Infra.Repositorios;
public class FilaEnviarCacambaRepositorio : IFilaEnviarCacambaRepositorio
{
    private readonly QueueClient _queueClient;
    private readonly StorageContextSettings _storageContext;

    public FilaEnviarCacambaRepositorio(IOptions<StorageContextSettings> storageContext)
    {
        _storageContext = storageContext.Value;
        _queueClient = new(_storageContext.ConnectionString, _storageContext.FilaEnviarCacamba);
        new QueueClientOptions
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
