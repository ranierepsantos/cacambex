namespace Domain.Autorizacao;
public class TokenConfiguracao
{
    public string Segredo { get; set; } = string.Empty;
    public string Emissor { get; set; } = string.Empty;
}
public class EmailConfiguracao
{
    public string Enviador { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Assunto { get; set; } = string.Empty;
}