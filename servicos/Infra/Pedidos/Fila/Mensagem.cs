using Azure.Storage.Queues;
using Domain.Pedidos.Fila;
using Infra.Dados;

namespace Infra.Pedidos.Fila;

public class Mensagem : IMensagem
{
    private readonly QueueClient _queueClient;

    public Mensagem(StorageContext storageContext)
    {

        _queueClient = new(storageContext.ConnectionString, storageContext.FilaSolicitaCTR);
    }
    public string AdicionarMensagem(string mensagem)
    {
        _queueClient.CreateIfNotExists();

        if (_queueClient.Exists())
        {
            try
            {
                _queueClient.SendMessage(mensagem);
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }
        return mensagem;
    }
}