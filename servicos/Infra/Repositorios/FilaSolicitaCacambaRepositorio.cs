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

    private readonly StorageContext _storageContext;

    public FilaSolicitaCacambaRepositorio(IOptions<StorageContext> storageContext)
    {
        _storageContext = storageContext.Value;
        _queueClient = new(_storageContext.ConnectionString, _storageContext.FilaEnviarCacamba,
        new QueueClientOptions
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
