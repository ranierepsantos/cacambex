using System.Net;
using Microsoft.Extensions.Configuration;

namespace Api.Notificacoes;

public static class ModeloEmail
{
    public static string RedefinirSenha(this IConfiguration configuration, string token)
    {
        string template = ObterTemplateHtml(configuration["EmailTemplate:RecoveryPassword"]);
        template = template.Replace("{{redirect}}", configuration["Auth:Redirect"] + token);
        template = template.Replace("{{logo}}", configuration["EmailTemplate:Logo"]);
        return template;
    }

    private static string ObterTemplateHtml(string path)
    {
        using WebClient web = new WebClient();
        return web.DownloadString(path);
    }
}
