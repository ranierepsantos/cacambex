using Domain.Pedidos.Comandos;
using FluentValidation;

namespace Domain.Pedidos.Validacoes;

public class VincularCacambaValidacao : AbstractValidator<VincularCacambaComando>
{
    public VincularCacambaValidacao()
    {
        RuleFor(x => x.PedidoId).NotNull().WithMessage("PedidoID é obrigatório.");
        RuleFor(x => x.CacambaId).NotNull().WithMessage("CaçambaID é obrigatório.");
    }
}
