using Domain.Clientes.Agrecacao;
using Domain.Identidade.Agregacao;

namespace Domain.Identidade.Interfaces
{
    public interface IAutoCadastroRepositorio
    {
        Task IncluirAutoCadastroUsuario(Usuario usuario);
        Task IncluirAutoCadastroCliente(Cliente cliente);
    }
}