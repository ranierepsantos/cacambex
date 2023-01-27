using Domain.Compartilhado;

namespace Domain.Clientes.Agrecacao;
public class EnderecoEntrega : Entidade
{
    public string CEP { get; private set; } = string.Empty;
    public string Logradouro { get; private set; } = string.Empty;
    public string Numero { get; private set; } = string.Empty;
    public string? Complemento { get; private set; }
    public string Bairro { get; private set; } = string.Empty;
    public string Cidade { get; private set; } = string.Empty;
    private string _uf = string.Empty;
    public string UF
    {
        get { return _uf; }
        set { _uf = value.ToUpper(); }
    }
    protected EnderecoEntrega() { }
    public EnderecoEntrega(string cep, string logradouro, string numero, string bairro, string cidade, string uf, string complemento)
    {
        CEP = cep;
        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
        Bairro = bairro;
        Cidade = cidade;
        UF = uf;
    }
    public void AtualizarEnderecoEntrega(
        string cep,
        string logradouro,
        string numero,
        string complemento,
        string bairro,
        string cidade,
        string uf)
    {
        CEP = cep;
        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
        Bairro = bairro;
        Cidade = cidade;
        UF = uf;
    }
}