namespace Domain.Cacambas.Consultas;
public class ConsultarCacamba
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public string Sort { get; set; } = "desc";
    public bool Ativo { get; set; }
}
