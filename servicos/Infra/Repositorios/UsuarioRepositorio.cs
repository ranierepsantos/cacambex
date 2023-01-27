using Infra.Dados;
using Microsoft.EntityFrameworkCore;
using Domain.Identidade.Interfaces;
using Domain.Identidade.Agregacao;

namespace Infra.Repositorios;

public class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly DataContext _db;
    public UsuarioRepositorio(DataContext db)
    {
        _db = db;
    }
    public async Task IncluirUsuario(Usuario usuario)
    {
        await _db.Usuarios.AddAsync(usuario);
        await _db.SaveChangesAsync();
    }
    public async Task<bool> UsuarioExistePorID(int id)
    {
        return await _db.Usuarios.AnyAsync(x => x.Id == id);

    }
    public async Task AtualizarUsuario(Usuario usuario)
    {
        _db.Entry(usuario).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }
    public async Task DeletarUsuario(Usuario usuario)
    {
        _db.Entry(usuario).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }

    public Usuario? ObterPorId(int id) => _db.Usuarios.FirstOrDefault(x => x.Id == id);

    public async Task<bool> ExisteEmail(string email)
    {
        return await _db.Usuarios.AnyAsync(x => x.Email == email);
    }

    public async Task<Usuario?> ObterPorEmail(string email) => await _db.Usuarios.FirstOrDefaultAsync(x => x.Email == email);
}
