using System.Linq.Expressions;
using Domain.Clientes.Agrecacao;
using Domain.Clientes.Enumeraveis;

namespace Domain.Clientes.Visualizacoes;

public class VisualizarCliente
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Documento { get; set; } = string.Empty;
    public TipoDocumento TipoDocumento { get; set; }
    public DateTime DataNascimento { get; set; }
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Contribuinte { get; set; } = string.Empty;
    public string Pessoa_fisica { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public EnderecoCobranca? EnderecoCobranca { get; set; }
    public List<EnderecoEntrega>? EnderecosEntrega { get; set; }
}
public static class VisualizarClienteExtensao
{
    public static Expression<Func<Cliente, VisualizarCliente>> ToView() => x => new VisualizarCliente
    {
        Id = x.Id,
        Nome = x.Nome,
        Documento = x.Documento,
        TipoDocumento = x.TipoDocumento,
        DataNascimento = x.DataNascimento,
        Telefone = x.Telefone,
        Email = x.Email,
        Contribuinte = x.Contribuinte,
        Pessoa_fisica = x.Pessoa_fisica,
        Ativo = x.Ativo,
        EnderecoCobranca = x.EnderecoCobranca,
        EnderecosEntrega = x.EnderecosEntrega
    };
}