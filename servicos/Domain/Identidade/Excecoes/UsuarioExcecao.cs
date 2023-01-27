using System.Runtime.Serialization;

namespace Domain.Identidade.Excecoes;
public class UsuarioExcecao : Exception
{
    public UsuarioExcecao()
    {
    }

    public UsuarioExcecao(string? message) : base(message)
    {
    }
    public UsuarioExcecao(string? message, string? message2) : base(message)
    {
    }

    public UsuarioExcecao(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected UsuarioExcecao(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}