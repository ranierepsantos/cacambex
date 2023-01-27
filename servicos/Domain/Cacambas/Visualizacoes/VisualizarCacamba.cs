using System.Linq.Expressions;
using Domain.Cacambas.Agregacao;
using Domain.Cacambas.Enumeraveis;

namespace Domain.Cacambas.Visualizacoes;

public class VisualizarCacamba
{
    public int Id { get; set; }
    public string? Numero { get; set; }
    public string? Volume { get; set; }
    public decimal Preco { get; set; }
    public Status Status { get; set; }
    public long nCodServ { get; set; }
    public string cCodIntServ { get; set; } = string.Empty;
}
public static class VisualizarCacambaExtensao
{
    public static Expression<Func<Cacamba, VisualizarCacamba>> ToView() => x => new VisualizarCacamba
    {
        Id = x.Id,
        Numero = x.Numero,
        Preco = x.Preco,
        Volume = x.Volume,
        Status = x.Status,
        nCodServ = x.nCodServ,
        cCodIntServ = x.cCodIntServ
    };
}
