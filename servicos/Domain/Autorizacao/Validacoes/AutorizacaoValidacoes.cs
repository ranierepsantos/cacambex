using Domain.Autorizacao.Comandos;
using FluentValidation;

namespace Domain.Autorizacao.Validacoes;
public class AutorizacaoUsuarioValidacao : AbstractValidator<AutenticacaoUsuarioComando>
{
    public AutorizacaoUsuarioValidacao()
    {
        RuleFor(x => x.Email).EmailAddress().WithMessage("E-mail inválido").Length(0, 255).WithMessage("E-mail deve ter no máximo 255 caracteres.");
        RuleFor(x => x.Senha)
            .NotNull().WithMessage("Senha é obrigatório.")
            .NotEmpty().WithMessage("Senha não pode ser vazio.")
            .Length(6, 6).WithMessage("Senha deve ter 6 caracteres.");
    }
}
public class EsqueciSenhaValidacao : AbstractValidator<EsqueciSenhaComando>
{
    public EsqueciSenhaValidacao()
    {
        RuleFor(x => x.Email).EmailAddress().WithMessage("E-mail inválido").Length(0, 255).WithMessage("E-mail deve ter no máximo 255 caracteres.");
    }
}
public class AlterarSenhaValidacao : AbstractValidator<AlterarSenhaComando>
{
    public AlterarSenhaValidacao()
    {
        RuleFor(x => x.NovaSenha)
            .NotNull().WithMessage("NovaSenha é obrigatório.")
            .NotEmpty().WithMessage("NovaSenha não pode ser vazio.")
            .Length(6, 6).WithMessage("NovaSenha deve ter 6 caracteres.");

        RuleFor(x => x.ConfirmarNovaSenha)
            .NotNull().WithMessage("ConfirmarNovaSenha é obrigatório.")
            .NotEmpty().WithMessage("ConfirmarNovaSenha não pode ser vazia.")
            .Equal(x => x.NovaSenha).WithMessage("As senhas devem ser idênticas.")
            .Length(0, 255).WithMessage("Senha deve ter 6 caracteres.");
    }
}