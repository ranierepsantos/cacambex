using Domain.Clientes.Enumeraveis;
using Domain.Clientes.Excecoes;
using Domain.Compartilhado;

namespace Domain.Clientes.Agrecacao;

public class Cliente : Entidade, IAggregateRoot
{
    protected Cliente() { }
    public string Codigo_cliente_integracao { get; private set; } = string.Empty;
    public long Codigo_cliente_omie { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Documento { get; private set; } = string.Empty;
    public DateTime DataNascimento { get; private set; }
    public string Telefone { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    private string _contribuinte = string.Empty;
    public string Contribuinte
    {
        get { return _contribuinte; }
        set { _contribuinte = value.ToUpper(); }
    }
    public EnderecoCobranca EnderecoCobranca { get; private set; } = null!;
    public List<EnderecoEntrega> EnderecosEntrega { get; private set; }
    private string _pessoa_fisica = string.Empty;
    public string Pessoa_fisica
    {
        get { return _pessoa_fisica; }
    }
    private TipoDocumento _tipoDocumento;
    public TipoDocumento TipoDocumento
    {
        get { return _tipoDocumento; }
        set
        {
            _tipoDocumento = value;
            _pessoa_fisica = value == TipoDocumento.Cpf ? "S" : "N";
        }
    }
    public bool Ativo { get; private set; }



    public Cliente(
        string nome,
        string documento,
        TipoDocumento tipoDocumento,
        DateTime dataNascimento,
        string telefone,
        string email,
        string contribuinte,
        EnderecoCobranca enderecoCobranca,
        List<EnderecoEntrega> enderecosEntrega)
    {
        Nome = nome;
        Documento = documento;
        TipoDocumento = tipoDocumento;
        DataNascimento = dataNascimento;
        Telefone = telefone;
        Email = email;
        Contribuinte = contribuinte;
        Codigo_cliente_integracao = documento;
        EnderecoCobranca = enderecoCobranca;
        EnderecosEntrega = enderecosEntrega;
        Ativo = true;
    }
    public void AlterarCliente(
        string nome,
        string email,
        string telefone,
        DateTime datanascimento,
        string contribuinte,
        EnderecoCobranca? enderecoCobranca)
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        DataNascimento = datanascimento;
        Contribuinte = contribuinte;
        EnderecoCobranca = enderecoCobranca != null ? enderecoCobranca : EnderecoCobranca;
    }
    public void AlterarEnderecoEntrega(EnderecoEntrega enderecoEntrega)
    {
        var end = EnderecosEntrega.FirstOrDefault(x => x.Id == enderecoEntrega.Id);
        end = enderecoEntrega;
    }
    public void AdicionarEnderecoEntrega(EnderecoEntrega enderecoEntrega) => EnderecosEntrega.Add(enderecoEntrega);
    public void RemoverEnderecoEntrega(EnderecoEntrega enderecoEntrega) => EnderecosEntrega.Remove(enderecoEntrega);
    public void AtualizarIdOmie(long idOmie)
    {
        if (idOmie is 0)
            throw new ClienteExcecao("Codigo cliente omie deve ser maior que zero");
        Codigo_cliente_omie = idOmie;
    }
    public void ExcluirCliente(Cliente cliente)
    {
        if (cliente.Ativo is false)
            throw new ClienteExcecao(nameof(cliente.Ativo), "Cliente já está inativo. Informe um cliente ativo.");
        cliente.Ativo = false;
    }
}