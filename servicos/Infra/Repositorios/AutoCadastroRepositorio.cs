using Infra.Dados;
using Domain.Identidade.Interfaces;
using Domain.Identidade.Agregacao;
using Domain.Clientes.Agrecacao;

namespace Infra.Repositorios;

public class AutoCadastroRepositorio : IAutoCadastroRepositorio
{
    private readonly DataContext _db;
    public AutoCadastroRepositorio(DataContext db)
    {
        _db = db;
    }
    public async Task IncluirAutoCadastroCliente(Cliente cliente)
    {
        await _db.Clientes.AddAsync(cliente);
        await _db.SaveChangesAsync();
    }

    public async Task IncluirAutoCadastroUsuario(Usuario usuario)
    {
        await _db.Usuarios.AddAsync(usuario);
        await _db.SaveChangesAsync();
    }
}
