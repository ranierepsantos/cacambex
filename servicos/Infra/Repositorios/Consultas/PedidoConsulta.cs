using Domain.Pedidos.Consultas;
using Domain.Pedidos.Visualizacoes;
using Infra.Dados;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositorios.Consultas;

public class PedidoConsulta : IPedidoConsulta
{
    private readonly DataContext _db;

    public PedidoConsulta(DataContext db)
    {
        _db = db;
    }
    public dynamic? ObterPorId(int id) => _db.Pedidos
                    .Include(x => x.Cliente)
                    .Include(x => x.PedidoItem)
                    .ThenInclude(x => x.Cacamba)
                    .Include(x => x.EnderecoEntrega)
                    .Select(VisualizarPedidoExtensao.ToView())
                    .FirstOrDefault(x => x.Id == id);
}