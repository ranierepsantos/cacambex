namespace Infra.Dados;
public class StorageContext
{
    public string ConnectionString { get; private set; } = string.Empty;
    public string FilaRetirarCacamba { get; private set; } = string.Empty;
    public string FilaSolicitaCTR { get; private set; } = string.Empty;
    public string FilaEnviarCacamba { get; private set; } = string.Empty;
}