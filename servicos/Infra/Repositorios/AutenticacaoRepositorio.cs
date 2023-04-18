using Domain.Autorizacao.Interfaces;
using Domain.Compartilhado;
using Domain.Identidade.Agregacao;
using Infra.Dados;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositorios;
public class AutenticacaoRepositorio : IAutenticacaoRepositorio
{
    private readonly DataContext _db;

    public AutenticacaoRepositorio(DataContext db)
    {
        _db = db;
    }

    // public string Encrypt(string senha)
    // {
    //     try
    //     {
    //         System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
    //         byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(senha);
    //         byte[] hash = md5.ComputeHash(inputBytes);
    //         System.Text.StringBuilder sb = new System.Text.StringBuilder();
    //         for (int i = 0; i < hash.Length; i++)
    //         {
    //             sb.Append(hash[i].ToString("X2"));
    //         }
    //         return sb.ToString();
    //     }
    //     catch (Exception ex)
    //     {
    //         throw new ArgumentException("Error:", nameof(ex));
    //     }
    // }

    public async Task<Usuario> Autenticacao(string email, string senha)
    {
        var usuario = await _db.Usuarios
                  .Where(x => x.Email == email)
                  .Where(x => x.Senha == senha.Encrypt())
                  .FirstOrDefaultAsync();
        return usuario;
    }
}