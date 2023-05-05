using Domain.Cacambas.Comandos;
using FluentValidation;

namespace Domain.Cacambas.Validacoes;
public class NovaCacambaValidacao : AbstractValidator<NovaCacambaComando>
{
    public NovaCacambaValidacao()
    {
        RuleFor(x => x.Numero)
            .NotNull().WithMessage("Número é obrigatório!")
            .NotEmpty().WithMessage("Número não pode ser vazio!");

        RuleFor(x => x.Volume)
            .NotNull().WithMessage("Volume é obrigatório!")
            .NotEmpty().WithMessage("Volume não pode ser vazio!");

        //RuleFor(x => x.Preco)
        //    .NotNull().WithMessage("Preço é obrigatório!")
        //    .NotEmpty().WithMessage("Preço não pode ser vazio!")
        //    .GreaterThan(0).WithMessage("Preço deve ser maior de zero!");
    }
}
public class AtualizarCacambaValidacao : AbstractValidator<AtualizarCacambaComando>
{
    public AtualizarCacambaValidacao()
    {
        RuleFor(x => x.Numero)
            .NotNull().WithMessage("Número é obrigatório!")
            .NotEmpty().WithMessage("Número não pode ser vazio!");

        RuleFor(x => x.Volume)
            .NotNull().WithMessage("Volume é obrigatório!")
            .NotEmpty().WithMessage("Volume não pode ser vazio!");

        //RuleFor(x => x.Preco)
        //    .NotNull().WithMessage("Preço é obrigatório!")
        //    .NotEmpty().WithMessage("Preço não pode ser vazio!")
        //    .GreaterThan(0).WithMessage("Preço deve ser maior de zero!");
    }
}