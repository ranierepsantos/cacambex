using Domain.TipoCacambas.Agregacao;
using System.Linq.Expressions;

namespace Domain.TipoCacambas.Visualizacoes
{
    public class VisualizarTipoCacamba
    {
        public int Id { get; set; }
        public string Volume { get; set; }
        public decimal Preco { get; set; }
        public Boolean Ativo { get; set; } = true;

    }

    public static class VisualizarTipoCacambaExtensao
    {
        public static Expression<Func<TipoCacamba, VisualizarTipoCacamba>> ToView() => x => new VisualizarTipoCacamba
        {
            Id  = x.Id,   
            Volume = x.Volume,
            Preco = x.Preco,
            Ativo = x.Ativo
        };
    }
}
