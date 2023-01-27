using Domain.Clientes.Comandos;
using FluentValidation;

namespace Domain.Clientes.Validacoes;
public class CriarClienteValidacao : AbstractValidator<CriarClienteComando>
{
    public CriarClienteValidacao()
    {
        RuleFor(x => x.Nome).NotEmpty().NotNull().WithMessage("Campo Nome não pode ser vazio!").MaximumLength(255);
        RuleFor(x => x.Telefone).NotEmpty().NotNull().WithMessage("Campo Telefone não pode ser vazio!").MaximumLength(255);
        RuleFor(x => x.Email).EmailAddress().WithMessage("Campo Email não pode ser vazio!").MaximumLength(255);
        RuleFor(x => x.EnderecoCobranca.CEP).NotEmpty().WithMessage("Campo CEP não pode ser vazio!").MinimumLength(8).WithMessage("CEP deve conter 8 números!").MaximumLength(9);
        RuleFor(x => x.EnderecoCobranca.Logradouro).NotEmpty().WithMessage("Campo Logradouro não pode ser vazio!").MaximumLength(255);
        RuleFor(x => x.EnderecoCobranca.Complemento).MaximumLength(255).WithMessage("Campo Complemento não pode ter mais de 255 caracteres!");
        RuleFor(x => x.EnderecoCobranca.Numero).NotEmpty().WithMessage("Campo Numero não pode ser vazio!").MaximumLength(255);
        RuleFor(x => x.EnderecoCobranca.Bairro).NotEmpty().WithMessage("Campo Bairro não pode ser vazio!").MaximumLength(255);
        RuleFor(x => x.EnderecoCobranca.Cidade).NotEmpty().WithMessage("Campo Cidade não pode ser vazio!").MaximumLength(255);
        RuleFor(x => x.EnderecoCobranca.UF).NotEmpty().WithMessage("Campo UF não pode ser vazio!").MinimumLength(2).MaximumLength(2).WithMessage("UF deve conter 2 caracteres. Ex: MG, SP..!").MaximumLength(255);

        RuleForEach(x => x.EnderecosEntrega).ChildRules(
            x =>
            {
                x.RuleFor(endereco => endereco.CEP).NotEmpty().NotNull().WithMessage("Campo CEP não pode ser vazio!").MinimumLength(8).WithMessage("CEP deve conter 8 números!").MaximumLength(9);
                x.RuleFor(endereco => endereco.Logradouro).NotEmpty().NotNull().WithMessage("Campo Logradouro não pode ser vazio!").MaximumLength(255);
                x.RuleFor(endereco => endereco.Numero).NotEmpty().NotNull().WithMessage("Campo Número não pode ser vazio!").MaximumLength(255);
                x.RuleFor(endereco => endereco.Complemento).MaximumLength(255).WithMessage("Campo Complemento não pode ter mais de 255 caracteres!");
                x.RuleFor(endereco => endereco.Bairro).NotEmpty().NotNull().WithMessage("Campo Bairro não pode ser vazio!").MaximumLength(255);
                x.RuleFor(endereco => endereco.Cidade).NotEmpty().NotNull().WithMessage("Campo Cidade não pode ser vazio!").MaximumLength(255);
                x.RuleFor(endereco => endereco.UF).NotEmpty().NotNull().WithMessage("Campo UF não pode ser vazio!").MinimumLength(2).MaximumLength(2).WithMessage("UF deve conter dois caracteres. Ex: MG, SP..!").MaximumLength(255);
            }
        );
    }
}
public class AtualizarClienteValidacao : AbstractValidator<AtualizarClienteComando>
{
    public AtualizarClienteValidacao()
    {
        RuleFor(x => x.Nome).NotEmpty().NotNull().WithMessage("Campo Nome não pode ser vazio!").MaximumLength(255);
        RuleFor(x => x.Telefone).NotEmpty().NotNull().WithMessage("Campo Telefone não pode ser vazio!").MaximumLength(255);
        RuleFor(x => x.Email).EmailAddress().WithMessage("Campo Email não pode ser vazio!").MaximumLength(255);
        RuleFor(x => x.EnderecoCobranca.CEP).NotEmpty().WithMessage("Campo CEP não pode ser vazio!").MinimumLength(8).WithMessage("CEP deve conter 8 números!").MaximumLength(9);
        RuleFor(x => x.EnderecoCobranca.Logradouro).NotEmpty().WithMessage("Campo Logradouro não pode ser vazio!").MaximumLength(255);
        RuleFor(x => x.EnderecoCobranca.Complemento).MaximumLength(255).WithMessage("Campo Complemento não pode ter mais de 255 caracteres!");
        RuleFor(x => x.EnderecoCobranca.Numero).NotEmpty().WithMessage("Campo Numero não pode ser vazio!").MaximumLength(255);
        RuleFor(x => x.EnderecoCobranca.Bairro).NotEmpty().WithMessage("Campo Bairro não pode ser vazio!").MaximumLength(255);
        RuleFor(x => x.EnderecoCobranca.Cidade).NotEmpty().WithMessage("Campo Cidade não pode ser vazio!").MaximumLength(255);
        RuleFor(x => x.EnderecoCobranca.UF).NotEmpty().WithMessage("Campo UF não pode ser vazio!").MinimumLength(2).MaximumLength(2).WithMessage("UF deve conter 2 caracteres. Ex: MG, SP..!").MaximumLength(255);

        RuleForEach(x => x.EnderecosEntrega).ChildRules(
            x =>
            {
                x.RuleFor(endereco => endereco.CEP).NotEmpty().NotNull().WithMessage("Campo CEP não pode ser vazio!").MinimumLength(8).WithMessage("CEP deve conter 8 números!").MaximumLength(9);
                x.RuleFor(endereco => endereco.Logradouro).NotEmpty().NotNull().WithMessage("Campo Logradouro não pode ser vazio!").MaximumLength(255);
                x.RuleFor(endereco => endereco.Numero).NotEmpty().NotNull().WithMessage("Campo Número não pode ser vazio!").MaximumLength(255);
                x.RuleFor(endereco => endereco.Complemento).MaximumLength(255).WithMessage("Campo Complemento não pode ter mais de 255 caracteres!");
                x.RuleFor(endereco => endereco.Bairro).NotEmpty().NotNull().WithMessage("Campo Bairro não pode ser vazio!").MaximumLength(255);
                x.RuleFor(endereco => endereco.Cidade).NotEmpty().NotNull().WithMessage("Campo Cidade não pode ser vazio!").MaximumLength(255);
                x.RuleFor(endereco => endereco.UF).NotEmpty().NotNull().WithMessage("Campo UF não pode ser vazio!").MinimumLength(2).MaximumLength(2).WithMessage("UF deve conter dois caracteres. Ex: MG, SP..!").MaximumLength(255);
            }
        );
    }
}
public class NovoEnderecoValidacao : AbstractValidator<CriarEnderecoEntregaComClienteIdComando>
{
    public NovoEnderecoValidacao()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(endereco => endereco.CEP).NotEmpty().NotNull().WithMessage("Campo CEP não pode ser vazio!").MinimumLength(8).WithMessage("CEP deve conter 8 números.").MaximumLength(9);
        RuleFor(endereco => endereco.Logradouro).NotEmpty().NotNull().WithMessage("Campo Logradouro não pode ser vazio!").MaximumLength(255);
        RuleFor(endereco => endereco.Numero).NotEmpty().NotNull().WithMessage("Campo Número não pode ser vazio!").MaximumLength(255);
        RuleFor(endereco => endereco.Complemento).MaximumLength(255).WithMessage("Campo Complemento não pode ter mais de 255 caracteres!");
        RuleFor(endereco => endereco.Bairro).NotEmpty().NotNull().WithMessage("Campo Bairro não pode ser vazio!").MaximumLength(255);
        RuleFor(endereco => endereco.Cidade).NotEmpty().NotNull().WithMessage("Campo Cidade não pode ser vazio!").MaximumLength(255);
        RuleFor(endereco => endereco.UF).NotEmpty().NotNull().WithMessage("Campo UF não pode ser vazio!").MinimumLength(2).MaximumLength(2).WithMessage("UF deve conter dois caracteres. Ex: MG, SP..!").MaximumLength(255);
    }
}