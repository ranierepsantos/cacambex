using Domain.Clientes.Consultas;
using Domain.Clientes.Visualizacoes;
using Infra.Dados;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositorios.Consultas;

public class ClienteConsulta : IClienteConsulta
{
    private readonly DataContext _db;

    public ClienteConsulta(DataContext db)
    {
        _db = db;
    }
    public dynamic? ObterPorId(int id) => _db.Clientes
                    .Include(x => x.EnderecosEntrega)
                    .Select(VisualizarClienteExtensao.ToView())
                    .FirstOrDefault(x => x.Id == id);
}