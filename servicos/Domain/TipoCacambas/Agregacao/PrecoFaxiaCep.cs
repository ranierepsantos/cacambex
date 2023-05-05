using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domain.Compartilhado;

namespace Domain.TipoCacambas.Agregacao
{
    public sealed class PrecoFaixaCep : Entidade
    {
        public int TipoCacambaId {get; private set;} = 0;
        public string CepInicial {get; private set;} = string.Empty;
        public string CepFinal {get; private set;} = string.Empty;
        public decimal Preco { get; private set; } = decimal.Zero;

        public PrecoFaixaCep(int tipoCacambaId, string cepInicial, string cepFinal, decimal preco)
        {
            TipoCacambaId= tipoCacambaId;
            CepInicial = cepInicial;
            CepFinal = cepFinal;
            Preco = preco;
        }

        public void Alterar(string cepInicial, string cepFinal, decimal preco)
        {
            CepInicial= cepInicial;
            CepFinal= cepFinal;
            Preco = preco;
        }
        
    }
}
