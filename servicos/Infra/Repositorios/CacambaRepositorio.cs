using Domain.Cacambas.Interface;
using Infra.Dados;
using Microsoft.EntityFrameworkCore;
using Domain.Cacambas.Agregacao;
using Domain.Cacambas.Enumeraveis;

namespace Infra.Repositorios;

public class CacambaRepositorio : ICacambaRepositorio
{
    private readonly DataContext _db;
    public CacambaRepositorio(DataContext db)
    {
        _db = db;
    }

    public async Task IncluirCacamba(Cacamba cacamba)
    {
        await _db.Cacambas.AddAsync(cacamba);
        await _db.SaveChangesAsync();
    }
    public async Task<bool> CacambaExistePorID(int id)
    {
        return await _db.Cacambas.AnyAsync(x => x.Id == id);
    }
    public async Task AtualizarCacamba(Cacamba cacamba)
    {
        _db.Entry(cacamba).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }
    public async Task DeletarCacamba(Cacamba cacamba)
    {
        _db.Entry(cacamba).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }

    public Cacamba? ObterPorId(int id) => _db.Cacambas.FirstOrDefault(x => x.Id == id);

    public async Task<bool> CacambaExiste(string numero)
    {
        return await _db.Cacambas.AnyAsync(x => x.Numero == numero);

    }
    public Cacamba? ObterPorNumero(string numero) => _db.Cacambas.FirstOrDefault(x => x.Numero == numero);

    public Cacamba? ObterPorVolume(string volume) => _db.Cacambas
                                                        .Where(x => x.Ativo == true)
                                                        .FirstOrDefault(x => x.Volume == volume);
}
