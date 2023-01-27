using Domain.Identidade.Agregacao;

namespace Domain.Autorizacao.Interfaces;
public interface IJwtRepositorio
{
    string GerarToken(Usuario usuario, int clienteId);
}