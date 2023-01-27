using Domain.Compartilhado;
using Domain.Identidade.Enumeraveis;
using Domain.Identidade.Excecoes;

namespace Domain.Identidade.Agregacao;

public class Usuario : Entidade
{
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Senha { get; private set; } = string.Empty;
    public bool Ativo { get; private set; }
    public Funcao Funcao { get; private set; }
    protected Usuario() { }
    public Usuario(string nome, string email, Funcao funcao)
    {
        Nome = nome;
        Email = email;
        Funcao = funcao;
        Ativo = true;
    }
    public void AtualizarUsuario(string nome, string email, Funcao funcao)
    {
        Nome = nome;
        Email = email;
        Funcao = funcao;
    }
    public void AlterarSenha(string senha) => Senha = senha.Encrypt();
    public Usuario(string nome, string email, Funcao funcao, string senha)
    {
        Nome = nome;
        Email = email;
        Funcao = funcao;
        Ativo = true;
        Senha = senha.Encrypt();
    }
    public void ExcluirUsuario(Usuario usuario)
    {
        if (usuario.Ativo == false)
            throw new UsuarioExcecao(nameof(usuario.Ativo), "Usuario já está inativo. Informe um Usuario ativo.");
        usuario.Ativo = false;
    }

}