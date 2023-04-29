using Domain.TipoCacambas.Comandos;
using FluentValidation;

namespace Domain.Cacambas.Validacoes;

public class AtualizarTipoCacambaValidacao : AbstractValidator<AtualizarTipoCacambaComando>
{
    public AtualizarTipoCacambaValidacao()
    {
        RuleFor(x => x.Volume)
            .NotNull().WithMessage("Volume é obrigatório!")
            .NotEmpty().WithMessage("Volume não pode ser vazio!");

        RuleFor(x => x.Preco)
            .NotNull().WithMessage("Preço é obrigatório!")
            .NotEmpty().WithMessage("Preço não pode ser vazio!")
            .GreaterThan(0).WithMessage("Preço deve ser maior de zero!");
    }
}