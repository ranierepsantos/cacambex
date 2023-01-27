using System.Text.Json;
using Azure.Storage.Queues;
using Domain.AzureStorage;
using Domain.ColetasOnline;
using Infra.Dados;

namespace Infra.Repositorios;
public class FilaSolicitaCacambaRepositorio : IFilaSolicitaCacambaRepositorio
{
    private readonly QueueClient _queueClient;

    public FilaSolicitaCacambaRepositorio(StorageContext _storageContext)
    {
        _queueClient = new(_storageContext.ConnectionString, _storageContext.FilaSolicitaCTR, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });
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
