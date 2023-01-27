namespace Domain.Clientes.Consultas;

public interface IClienteConsulta
{
    public dynamic? ObterPorId(int id);
}
public class ConsultarClientes
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;

}
