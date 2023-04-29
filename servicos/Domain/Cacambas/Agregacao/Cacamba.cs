using Domain.Cacambas.Enumeraveis;
using Domain.Cacambas.Excecoes;
using Domain.Compartilhado;

namespace Domain.Cacambas.Agregacao;

public class Cacamba : Entidade
{
    public string Numero { get; private set; }
    public string Volume { get; private set; }
    public decimal Preco { get; private set; }
    public Status Status { get; private set; }
    public long nCodServ { get; private set; }
    public string cCodIntServ { get; private set; } = string.Empty;
    public bool Ativo { get; private set; }

    public Cacamba(string numero, string volume, decimal preco)
    {
        Numero = numero;
        Volume = volume;
        Status = Status.Disponivel;
        Preco = preco;
        Ativo = true;
    }
    public void AtualizarCacamba(string numero, string volume, decimal preco)
    {
        Numero = numero;
        Volume = volume;
        Preco = preco;
    }
    public void AtualizarCodigosCacambaOmie(long nCodServ, string cCodIntServ)
    {
        if (nCodServ is 0) throw new CacambaExcecao("Codigo da cacamba nao pode ser zero!");
        this.nCodServ = nCodServ;
        if (string.IsNullOrEmpty(cCodIntServ)) throw new CacambaExcecao("Codigo interno da cacamba nao pode ser zero!");
        this.cCodIntServ = cCodIntServ;
    }
    public void ExcluirCacamba(Cacamba cacamba)
    {
        if (cacamba.Ativo == false)
            throw new CacambaExcecao(nameof(cacamba.Ativo), "cacamba já está inativo. Informe uma cacamba ativo.");
        cacamba.Ativo = false;
    }
    public void AlterarStatus(Cacamba cacamba)
    {
        if (cacamba.Status == Status.Alocado)
            Status = Status.Disponivel;
        else
            Status = Status.Alocado;
    }
}
