using Domain.Compartilhado;
using Domain.Pedidos.Interface;
using Domain.TipoCacambas.Consultas;
using Domain.TipoCacambas.Interface;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Pedidos.Consultas
{
    public record ConsultarPedidoDevolverCacambaComando(int dias) : IRequest<Resposta>;

    public class ConsultarPedidoDevolverCacambaHandler : IRequestHandler<ConsultarPedidoDevolverCacambaComando, Resposta>
    {
        private readonly ILogger<ConsultarPedidoDevolverCacambaHandler> _logger;
        private readonly IPedidoRepositorio _repositorio;

        public ConsultarPedidoDevolverCacambaHandler(ILogger<ConsultarPedidoDevolverCacambaHandler> logger,
                                                     IPedidoRepositorio repositorio)
        {
            _logger = logger;
            _repositorio = repositorio;
        }

        public Task<Resposta> Handle(ConsultarPedidoDevolverCacambaComando request, CancellationToken cancellationToken)
        {
            
            throw new NotImplementedException();
        }
    }
}
