using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Autorizacao;
using Domain.Autorizacao.Interfaces;
using Domain.Identidade.Agregacao;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infra.Repositorios;
public class TokenRepositorio : ITokenRepositorio
{
    private readonly TokenConfiguracao _tokenConfiguracao;

    public TokenRepositorio(IOptions<TokenConfiguracao> tokenConfiguracao)
    {
        _tokenConfiguracao = tokenConfiguracao.Value;
    }

    public string GerarToken(Usuario usuario, int clienteId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_tokenConfiguracao.Segredo);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Role, usuario.Funcao.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, clienteId.ToString()),
            }),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}