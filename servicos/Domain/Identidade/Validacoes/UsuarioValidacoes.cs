using Domain.Identidade.Comandos;
using FluentValidation;

namespace Domain.Identidade.Validacoes;
public class CriarUsuarioValidacao : AbstractValidator<CriarUsuarioComando>
{
    public CriarUsuarioValidacao()
    {
        RuleFor(x => x.Nome).NotEmpty().NotNull().WithMessage("O campo  Nome não pode ser vazio");
        RuleFor(x => x.Email).EmailAddress().NotEmpty().NotNull().WithMessage("O campo E-mail não pode ser vazio");
        // RuleFor(x => x.Funcao).IsInEnum().NotNull().NotEmpty().WithMessage("Uma função deve ser informada");
    }
}
public class AtualizarUsuarioValidacao : AbstractValidator<AtualizarUsuarioComando>
{
    public AtualizarUsuarioValidacao()
    {
        RuleFor(x => x.Nome).NotEmpty().NotNull().WithMessage("Um nome não pode ser vazio");
        // RuleFor(x => x.Funcao).IsInEnum().NotNull().NotEmpty().WithMessage("Uma função deve ser informada");
        RuleFor(x => x.Email).EmailAddress().NotNull().NotEmpty().WithMessage("Um e-mail deve ser informada");
    }
}
public class DeletarUsuarioValidacao : AbstractValidator<DeletarUsuarioComando>
{
    public DeletarUsuarioValidacao()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Um nome não pode ser vazio");
    }
}