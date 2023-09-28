namespace Infra.Dados;
public class StorageContextSettings
{
    public string ConnectionString { get; set; } = "DefaultEndpointsProtocol=https;AccountName=grcacambexbrbaf4;AccountKey=RC6l6AOTXoKPhcbSY8EXb5WchJsGbhAkG+NQdH6VPiP9KCBsgXOkhyNiJ69nDg8/bBiSIfOy+fjr+AStYBHFRw==;EndpointSuffix=core.windows.net";
    public string FilaRetirarCacamba { get; set; } = "retirarcacamba";
    public string FilaSolicitaCTR { get; set; } = "solicitactr";
    public string FilaEnviarCacamba { get; set; } = "enviarcacamba";
}