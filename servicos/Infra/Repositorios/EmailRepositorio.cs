using Domain.Autorizacao;
using Domain.Identidade.Agregacao;
using Domain.Identidade.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Infra.Repositorios;
public class EmailRepositorio : IEmailRepositorio
{
    private readonly EmailConfiguracao _emailConfiguracao;

    public EmailRepositorio(IOptions<EmailConfiguracao> emailConfiguracao)
    {
        _emailConfiguracao = emailConfiguracao.Value;
    }

    public async Task EnviarEmailRecuperarSenha(Usuario usuario, string jwtToken, string origin)
    {
        var email = new MimeMessage();
        string sender = _emailConfiguracao.Enviador;
        email.From.Add(MailboxAddress.Parse(sender));
        email.To.Add(MailboxAddress.Parse(usuario.Email));
        email.Subject = _emailConfiguracao.Assunto;

        var url = $"{origin}/#/identidade-acesso/resetar-senha/{jwtToken}";
        email.Body = new TextPart(TextFormat.Html)
        {
            Text = $"<h3>Olá {usuario.Nome}. Você está recebendo esse e-mail pois solicitou um recadrasto de senha no portal GP | Caçambex.</h3>" +
            $"<h3>Clique no botão abaixo para cadastrar uma nova senha:</h3> <a href='{url}'><input type='button' value='Alterar senha'/></a>" +
            $"<h3>Ou copie e cole esse link no seu navegador: <p>{url}</p></h3>" +
            $"<h3>Se você não solicitou esse recadrasto, por favor, ignore esse e-mail.</h3>"
        };

        using var smtp = new SmtpClient();
        smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate(sender, _emailConfiguracao.Senha);
        await smtp.SendAsync(email);
        smtp.Disconnect(true);
    }
}