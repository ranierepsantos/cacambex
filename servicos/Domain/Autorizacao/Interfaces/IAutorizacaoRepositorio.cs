using Domain.Identidade.Agregacao;

namespace Domain.Autorizacao.Interfaces;
public interface IAutenticacaoRepositorio
{
    Task<Usuario> Autenticacao(string email, string senha);
    // string Encrypt(string senha);
}