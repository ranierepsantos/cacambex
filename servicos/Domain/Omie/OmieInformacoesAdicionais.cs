namespace Domain.Omie;
public class OmieInformacoesAdicionais
{
    public OmieInformacoesAdicionais(long contaCorrente, string codigoCategoria)
    {
        ContaCorrente = contaCorrente;
        CodigoCategoria = codigoCategoria;
    }

    public long ContaCorrente { get; set; }
    public string CodigoCategoria { get; set; } = string.Empty;
}