using System.Text.Json;
using Azure.Storage.Queues;
using Domain.AzureStorage;
using Domain.ColetasOnline;
using Infra.Dados;
using Microsoft.Extensions.Options;

namespace Infra.Repositorios;
public class FilaRetirarCacambaRepositorio : IFilaRetirarCacambaRepositorio
{
    private readonly QueueClient _queueClient;

    private readonly StorageContext _storageContext;

    public FilaRetirarCacambaRepositorio(IOptions<StorageContext> storageContext)
    {
        _storageContext = storageContext.Value;
        _queueClient = new(_storageContext.ConnectionString, _storageContext.FilaEnviarCacamba,
        new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });
    }
    public async Task FilaRetiraCacamba(RetirarCacambaRequest retirarCacamba)
    {
        _queueClient.CreateIfNotExists();
        var retirarCacambaString = JsonSerializer.Serialize(retirarCacamba);
        if (_queueClient.Exists())
        {
            try
            {
                await _queueClient.SendMessageAsync(retirarCacambaString);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
