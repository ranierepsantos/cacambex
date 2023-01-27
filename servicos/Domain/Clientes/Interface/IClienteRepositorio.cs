using Domain.Clientes.Agrecacao;

namespace Domain.Clientes.Interface
{
    public interface IClienteRepositorio
    {
        Task IncluirCliente(Cliente cliente);
        Task<bool> ClienteExistePorID(int id);
        Task<bool> DocumentoExiste(string documento);
        Task AtualizarCliente(Cliente cliente);
        Task DeletarCliente(Cliente cliente);
        Cliente? ObterPorId(int id);
        Cliente? ObterPorCNPJ_CPF(string cnpj_cpf);
        Cliente? ObterPorEmail(string email);
        Cliente? ObterClientePorIdComEndereco(int id);
        EnderecoEntrega? ObterEnderecoEntregaDoCliente(int id);
    }
}