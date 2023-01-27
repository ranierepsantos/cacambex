using System.Runtime.Serialization;

namespace Domain.Cacambas.Excecoes;
public class CacambaExcecao : Exception
{
    public CacambaExcecao()
    {
    }

    public CacambaExcecao(string? message) : base(message)
    {
    }
    public CacambaExcecao(string? message, string? message2) : base(message)
    {
    }

    public CacambaExcecao(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected CacambaExcecao(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}