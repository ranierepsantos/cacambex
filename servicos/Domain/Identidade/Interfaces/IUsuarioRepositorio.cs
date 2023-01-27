using Domain.Identidade.Agregacao;

namespace Domain.Identidade.Interfaces
{
    public interface IUsuarioRepositorio
    {
        Task IncluirUsuario(Usuario usuario);
        Task AtualizarUsuario(Usuario usuario);
        Task DeletarUsuario(Usuario usuario);
        Usuario? ObterPorId(int id);
        Task<Usuario?> ObterPorEmail(string email);
        Task<bool> ExisteEmail(string email);
    }
}