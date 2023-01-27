namespace Infra.Dados;
public class StorageContext
{
    public string ConnectionString { get; private set; }
    public string FilaRetirarCacamba { get; private set; }
    public string FilaSolicitaCTR { get; private set; }
    public string FilaEnviarCacamba { get; private set; }

    public StorageContext(string connectionString,
                          string queueRetirarCacamba,
                          string queueCTR,
                          string filaEnviarCacamba)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));
        if (string.IsNullOrEmpty(queueRetirarCacamba))
            throw new ArgumentNullException(nameof(queueRetirarCacamba));
        if (string.IsNullOrEmpty(queueCTR))
            throw new ArgumentNullException(nameof(queueCTR));
        if (string.IsNullOrEmpty(filaEnviarCacamba))
            throw new ArgumentNullException(nameof(filaEnviarCacamba));
        ConnectionString = connectionString;
        FilaRetirarCacamba = queueRetirarCacamba;
        FilaSolicitaCTR = queueCTR;
        FilaEnviarCacamba = filaEnviarCacamba;
    }
}