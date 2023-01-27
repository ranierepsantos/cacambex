using System.Runtime.Serialization;

namespace Domain.Pedidos.Execoes;

public class PedidoExcecoes : Exception
{
    public PedidoExcecoes()
    {
    }

    public PedidoExcecoes(string? message) : base(message)
    {
    }
    public PedidoExcecoes(string message, string? message2) : base(message)
    {
    }

    public PedidoExcecoes(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected PedidoExcecoes(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
