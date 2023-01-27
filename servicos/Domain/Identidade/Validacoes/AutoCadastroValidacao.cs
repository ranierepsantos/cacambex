using Domain.Identidade.Comandos;
using FluentValidation;

namespace Domain.Identidade.Validacoes;
public class AutoCadastroValidacao : AbstractValidator<NovoAutoCadastroComando>
{
    public AutoCadastroValidacao()
    {
        RuleFor(x => x.Cliente.Nome).NotEmpty().NotNull().WithMessage("Campo Nome não pode ser vazio");
        RuleFor(x => x.Cliente.Documento).NotEmpty().NotNull().MaximumLength(13).MinimumLength(11).WithMessage("Campo Documento deve conter entre 11 e 13 caracteres");
        // RuleFor(x => x.Cliente.TipoDocumento).IsInEnum().WithMessage("Campo Tipo Documento não pode ser vazio");
        RuleFor(x => x.Cliente.DataNascimento).NotEmpty().NotNull().WithMessage("Campo Data de nascimento não pode ser vazio");
        RuleFor(x => x.Cliente.Telefone).NotEmpty().NotNull().MaximumLength(20).WithMessage("Campo Telefone não pode ser vazio. Suporta apenas números, exemplo: (11)912345678");
        RuleFor(x => x.Cliente.Contribuinte).NotEmpty().NotNull().WithMessage("Campo contribuinte não pode ser vazio").Length(1, 1).WithMessage("Campos contribuinte deve ter 1 caracteres.");
        RuleFor(x => x.Cliente.Pessoa_fisica).NotEmpty().NotNull().WithMessage("Campo pessoa fisica não pode ser vazio").Length(1, 1).WithMessage("Campo pessoa fisica deve ter 1 caracteres.");
        RuleFor(x => x.Cliente.Email).EmailAddress().WithMessage("Campo Email não pode ser vazio");
        RuleFor(x => x.Senha).NotNull().NotEmpty().WithMessage("Uma senha deve ser fornecida.").Length(1, 255).WithMessage("Senha deve ter no máximo 255 caracteres.");
        RuleFor(x => x.ConfirmarSenha).NotNull().NotEmpty().WithMessage("Senha deve ser fornecida.").Equal(x => x.Senha).WithMessage("As senhas devem ser idênticas.")
        .Length(0, 255).WithMessage("Senha deve ter no máximo 255 caracteres.");
    }
}