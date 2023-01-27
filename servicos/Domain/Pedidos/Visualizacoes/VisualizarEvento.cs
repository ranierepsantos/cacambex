using Domain.Pedidos.Enumeraveis;
using Domain.Pedidos.Eventos;

namespace Domain.Pedidos.Visualizacoes
{
    public class VisualizarEvento
    {
        public DateTime Quando { get; set; }
        public string? Descricao { get; set; }
        public StatusPedido Status { get; set; }
        public string? Mensagem { get; set; }
    }
    public static class VisualizarEventoExtensao
    {
        public static VisualizarEvento ToView(this Evento evento)
        {
            return new VisualizarEvento()
            {
                Quando = evento.Quando,
                Descricao = evento.Descricao,
                Status = evento.Status,
                Mensagem = evento.Mensagem
            };
        }
        public static StatusPedido ResumirStatus(this IEnumerable<Evento> eventos)
        {
            return eventos.Any(y => y.Status == StatusPedido.Aguardando)
                                            ? StatusPedido.Aguardando
                                            : eventos.Any(y => y.Status == StatusPedido.ComFalhas)
                                            ? StatusPedido.ComFalhas
                                            : eventos.Any(y => y.Status == StatusPedido.NaoEmitido)
                                            ? StatusPedido.NaoEmitido
                                            : StatusPedido.Concluido;
        }
    }
}