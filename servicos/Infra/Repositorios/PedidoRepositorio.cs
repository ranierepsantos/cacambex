using Domain.Pedidos.Agregacao;
using Domain.Pedidos.Interface;
using Infra.Dados;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositorios;

public class PedidoRepositorio : IPedidoRepositorio
{
    private readonly DataContext _db;
    public PedidoRepositorio(DataContext db)
    {
        _db = db;
    }
    public async Task IncluirPedidoAsync(Pedido pedido)
    {
        await _db.Pedidos.AddAsync(pedido);
        await _db.SaveChangesAsync();
    }
    public async Task AtualizarPedidoAsync(Pedido pedido)
    {
        _db.Entry(pedido).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }
    public async Task DeletarPedidoAsync(Pedido pedido)
    {
        _db.Entry(pedido).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }
    public async Task<Pedido?> ObterPedidoPorIdAsync(int id) => await _db.Pedidos
        .Include(x => x.Cliente)
        .ThenInclude(x => x.EnderecoCobranca)
        .Include(x => x.EnderecoEntrega)
        .Include(x => x.PedidoItem)
        .ThenInclude(x => x.CTR)
        .Include(x => x.PedidoItem)
        .ThenInclude(x => x.RecolherItem)
        .Include(x => x.PedidoItem)
        .ThenInclude(x => x.ItemEntregue)
        .Include(x => x.PedidoItem)
        .ThenInclude(x => x.PedidoConcluido)
        .Include(x => x.PedidoItem)
        .ThenInclude(x => x.Cacamba)
        .Include(x => x.NotaFiscal)
        .FirstOrDefaultAsync(x => x.Id == id);
}
