using Domain.Compartilhado;
using Domain.TipoCacambas.Agregacao;
using Domain.TipoCacambas.Consultas;
using Domain.TipoCacambas.Interface;
using Domain.TipoCacambas.Visualizacoes;
using Infra.Dados;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repositorios
{
    public class TipoCacambaRepositorio : ITipoCacambaRepositorio
    {

        private readonly DataContext _db;
        public TipoCacambaRepositorio(DataContext db)
        {
            _db = db;
        }
        public async Task AtualizarTipoCacambaAsync(TipoCacamba tipoCacamba)
        {
            _db.TipoCacambas.Update(tipoCacamba);
            await _db.SaveChangesAsync();
        }

        public Task DeletarTipoCacambaAsync(TipoCacamba tipoCacamba)
        {
            throw new NotImplementedException();
        }

        public Task IncluirTipoCacambaAsync(TipoCacamba tipoCacamba)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TipoCacamba> ListarTodos()
        {
            return _db.TipoCacambas.AsNoTracking();

        }

        public async Task<TipoCacamba?> ObterTipoCacambaPorIdAsync(int id)
        {
            var data = await _db.TipoCacambas.FirstOrDefaultAsync(x => x.Id == id);
            return data;

        }

        public async Task<TipoCacamba?> ObterTipoCacambaPorVolumeAsync(string volume)
        {
            var data = await _db.TipoCacambas.FirstOrDefaultAsync(x => x.Volume == volume);
            return data;
        }
    }
}
