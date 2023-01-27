using System.Runtime.Serialization;

namespace Domain.Clientes.Excecoes;

public class ClienteExcecao : Exception
{
    public ClienteExcecao()
    {
    }

    public ClienteExcecao(string? message) : base(message)
    {
    }
    public ClienteExcecao(string? message, string? message2) : base(message)
    {
    }

    public ClienteExcecao(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected ClienteExcecao(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
