using System.Text.Json;
using Azure.Storage.Queues;
using Domain.AzureStorage;
using Domain.ColetasOnline;
using Infra.Dados;
using Microsoft.Extensions.Options;

namespace Infra.Repositorios;
public class FilaSolicitaCacambaRepositorio : IFilaSolicitaCacambaRepositorio
{
    private readonly QueueClient _queueClient;
    private readonly StorageContextSettings _storageContextSettings;

    public FilaSolicitaCacambaRepositorio(IOptions<StorageContextSettings> storageContextSettings)
    {
        _storageContextSettings = storageContextSettings.Value;
        _queueClient = new(
            _storageContextSettings.ConnectionString,
            _storageContextSettings.FilaSolicitaCTR,
            new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            }
        );
    }
    public async Task FilaSolicitaCTR(SolicitaCTRRequest ctr)
    {
        _queueClient.CreateIfNotExists();
        var ctrString = JsonSerializer.Serialize(ctr);
        if (_queueClient.Exists())
        {
            try
            {
                await _queueClient.SendMessageAsync(ctrString);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
