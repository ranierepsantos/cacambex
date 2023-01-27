using Domain.Clientes.Agrecacao;
using Domain.Clientes.Interface;
using Infra.Dados;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositorios;

public class ClienteRepositorio : IClienteRepositorio
{
    private readonly DataContext _db;
    public ClienteRepositorio(DataContext db)
    {
        _db = db;
    }
    public async Task IncluirCliente(Cliente cliente)
    {
        await _db.Clientes.AddAsync(cliente);
        await _db.SaveChangesAsync();
    }
    public async Task<bool> ClienteExistePorID(int id)
    {
        return await _db.Clientes.AnyAsync(x => x.Id == id);

    }
    public async Task<bool> DocumentoExiste(string documento)
    {
        return await _db.Clientes.AnyAsync(x => x.Documento == documento);
    }
    public async Task AtualizarCliente(Cliente cliente)
    {
        _db.Entry(cliente).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }
    public async Task DeletarCliente(Cliente cliente)
    {
        _db.Entry(cliente).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }

    public Cliente? ObterPorId(int id) => _db.Clientes.FirstOrDefault(x => x.Id == id);

    public Cliente? ObterPorCNPJ_CPF(string cnpj_cpf) => _db.Clientes.FirstOrDefault(x => x.Documento == cnpj_cpf);
    public Cliente? ObterPorEmail(string email) => _db.Clientes.FirstOrDefault(x => x.Email == email);

    public Cliente? ObterClientePorIdComEndereco(int id) =>
        _db.Clientes
        .Include(x => x.EnderecoCobranca)
        .Include(x => x.EnderecosEntrega)
        .FirstOrDefault(x => x.Id == id);

    public EnderecoEntrega? ObterEnderecoEntregaDoCliente(int id) => _db.EnderecosEntrega.FirstOrDefault(x => x.Id == id);
}
