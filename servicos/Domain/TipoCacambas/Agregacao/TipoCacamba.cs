using Domain.Compartilhado;

namespace Domain.TipoCacambas.Agregacao
{
    public sealed class TipoCacamba : Entidade
    {
        public string Volume { get; private set; }
        public decimal Preco { get; private set; }

        public Boolean Ativo {  get; private set; } = true;

        public TipoCacamba(string volume, decimal preco, bool ativo = true)
        {
            Volume = volume;
            Preco = preco;
            Ativo = ativo;
        }

        public TipoCacamba() { }

        public void AtualizarTipoCacamba(string volume, decimal preco, bool ativo)
        {
            Volume = volume;
            Preco = preco;
            Ativo = ativo;
        }

        public TipoCacamba(int id, string volume, decimal preco, bool ativo)
        {
            Id = id;
            Volume = volume;
            Preco = preco;
            Ativo = ativo;
        }

    }
}
