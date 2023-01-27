using Domain.Pedidos.Comandos;
using FluentValidation;

namespace Domain.Pedidos.Validacoes;

public class CtrValidacao : AbstractValidator<SolicitaCtrComando>
{

    public CtrValidacao()
    {
        RuleFor(x => x.PedidoId).NotNull().WithMessage("Pedido inválido.");
        RuleFor(x => x.ClasseResiduo).NotNull().WithMessage("Pedido inválido.");
    }
}

