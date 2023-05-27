namespace Domain.Pedidos.Consultas
{
    public interface IPedidoConsulta
    {
        public dynamic? ObterPorId(int id);
    }

    public class ConsultaPedidos
    {
        public int PageIndex { get; set; } = 0;

        public int PageSize { get; set; } = 10;

        public string NumeroCTR { get; set; } = "";

        public string DocumentoCliente { get; set; } = "";
        public string NomeCliente { get; set; } = "";
        //IMPLANTAR
        public string NotaFiscal { get; set; } = "";
        public DateTime? DataInicio { get; set; } = null;
        public DateTime? DataFim { get; set; } = null;
        public bool FiltrarPorData { get; set; } = false;
        public string Sort { get; set; } = "desc";
    }
}