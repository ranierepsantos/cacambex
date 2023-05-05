using Domain.Compartilhado;
using Domain.TipoCacambas.Agregacao;
using Domain.TipoCacambas.Consultas;
using Domain.TipoCacambas.Interface;
using Domain.TipoCacambas.Visualizacoes;
using Infra.Dados;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
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

        public IQueryable<TipoCacamba> ToQueryAsNoTracking()
        {
            return _db.Set<TipoCacamba>().AsNoTracking();

        }

        public IQueryable<TipoCacamba> ToQueryWithPrecoFaixaCepAsNoTracking(string cep = "")
        {
            var query = _db.TipoCacambas.AsNoTracking();

            if (!string.IsNullOrEmpty(cep))
                query = query.Include(p => p.PrecoFaixaCep.Where(c => c.CepInicial.CompareTo(cep) <= 0 && c.CepFinal.CompareTo(cep) >= 0));
            else
                query = query.Include(ic => ic.PrecoFaixaCep);

            Console.Write(JsonConvert.SerializeObject(query.ToList()));

            return query;

        }

        public async Task<TipoCacamba?> ObterTipoCacambaPorIdAsync(int id)
        {
            var data = await _db.TipoCacambas.FirstOrDefaultAsync(x => x.Id == id);
            return data;

        }


        public async Task<TipoCacamba?> ObterTipoCacambaPorIdComPrecoFaixaCepAsync(int id)
        {
            var data = await _db.TipoCacambas.Include(ic =>  ic.PrecoFaixaCep).FirstOrDefaultAsync(x => x.Id == id);
            return data;
        }

        public async Task<TipoCacamba?> ObterTipoCacambaPorVolumeAsync(string volume)
        {
            var data = await _db.TipoCacambas.FirstOrDefaultAsync(x => x.Volume == volume);
            return data;
        }
    }
}
