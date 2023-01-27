using System.Linq.Expressions;
using Domain.Identidade.Agregacao;
using Domain.Identidade.Enumeraveis;

namespace Domain.Identidade.Visualizacoes;

public class VisualizarUsuario
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Senha { get; set; }
    public bool Ativo { get; set; }
    public Funcao Funcao { get; set; }
}
public static class VisualizarUsuarioExtensao
{
    public static Expression<Func<Usuario, VisualizarUsuario>> ToView() => x => new VisualizarUsuario
    {
        Id = x.Id,
        Nome = x.Nome,
        Email = x.Email,
        Ativo = x.Ativo,
        Funcao = x.Funcao,
    };
}
