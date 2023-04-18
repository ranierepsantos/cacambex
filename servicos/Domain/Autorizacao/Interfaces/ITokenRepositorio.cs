using Domain.Identidade.Agregacao;

namespace Domain.Autorizacao.Interfaces;
public interface ITokenRepositorio
{
    string GerarToken(Usuario usuario, int clienteId);
}