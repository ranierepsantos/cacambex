namespace Infra.Dados;
public class StorageContextSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string FilaRetirarCacamba { get; set; } = string.Empty;
    public string FilaSolicitaCTR { get; set; } = string.Empty;
    public string FilaEnviarCacamba { get; set; } = string.Empty;
}