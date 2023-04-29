using Domain.Compartilhado;
using Domain.TipoCacambas.Agregacao;
using Domain.TipoCacambas.Consultas;
using Domain.TipoCacambas.Visualizacoes;

namespace Domain.TipoCacambas.Interface
{
    public interface ITipoCacambaRepositorio
    {
        Task IncluirTipoCacambaAsync(TipoCacamba tipoCacamba);
        Task AtualizarTipoCacambaAsync(TipoCacamba tipoCacamba);
        Task DeletarTipoCacambaAsync(TipoCacamba tipoCacamba);
        Task<TipoCacamba?> ObterTipoCacambaPorIdAsync(int id);

        Task<TipoCacamba?> ObterTipoCacambaPorVolumeAsync(string volume);

        IQueryable<TipoCacamba> ListarTodos();
    }

            
   
}
