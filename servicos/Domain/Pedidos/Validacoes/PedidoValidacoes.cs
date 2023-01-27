using Domain.Pedidos.Comandos;
using FluentValidation;

namespace Domain.Pedidos.Validacoes;
public class NovoPedidoValidacao : AbstractValidator<NovoPedidoComando>
{
    public NovoPedidoValidacao()
    {
        RuleFor(x => x.ClienteId).NotNull().WithMessage("Cliente é obrigatório.");
        RuleFor(x => x.EnderecoId).NotNull().WithMessage("Endereço de entrega é obrigatório.");
        RuleFor(x => x.TipoDePagamento).IsInEnum().WithMessage("Tipo de pagamento inválido.");
        RuleFor(x => x.VolumeCacamba).NotNull().WithMessage("Volume da caçamba é obrigatório.");
        RuleFor(x => x.Observacao).MaximumLength(255).WithMessage("Observação não pode ter mais de 255 caracteres!");
    }
}
public class AtualizarPedidoValidacao : AbstractValidator<AtualizarPedidoComando>
{
    public AtualizarPedidoValidacao()
    {
        RuleFor(x => x.PedidoId).NotNull().WithMessage("Pedido é obrigatório.");
        RuleFor(x => x.EnderecoId).NotNull().WithMessage("Endereço de entrega é obrigatório.");
        RuleFor(x => x.TipoDePagamento).IsInEnum().WithMessage("Tipo de pagamento inválido.");
        RuleFor(x => x.VolumeCacamba).NotNull().WithMessage("Ao menos um item é obrigatório.");
        RuleFor(x => x.Observacao).MaximumLength(255).WithMessage("Observação não pode ter mais de 255 caracteres!");
    }
}