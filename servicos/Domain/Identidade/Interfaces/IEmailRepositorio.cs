using Domain.Identidade.Agregacao;

namespace Domain.Identidade.Interfaces;
public interface IEmailRepositorio
{
    Task EnviarEmailRecuperarSenha(Usuario usuario, string jwtToken, string origin);
}