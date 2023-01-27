using Domain.Omie.Clientes.Interfaces;
using Domain.Omie.Clientes.OmieClienteRequests;
using Domain.Clientes.Agrecacao;
using Domain.Clientes.Interface;
using Domain.Compartilhado;
using Domain.Identidade.Agregacao;
using Domain.Identidade.Enumeraveis;
using Domain.Identidade.Interfaces;
using Domain.Identidade.Validacoes;
using Domain.Omie.Clientes.OmieClienteResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Identidade.Comandos;

public record NovoAutoCadastroComando(Cliente Cliente, string Senha, string ConfirmarSenha) : IRequest<Resposta>;

public class NovoAutoCadastro : IRequestHandler<NovoAutoCadastroComando, Resposta>
{
    private readonly ILogger<NovoAutoCadastro> _logger;
    private readonly IAutoCadastroRepositorio _autoCadastroRepositorio;
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    private readonly IClienteRepositorio _clienteRepositorio;
    private readonly IOmieClientes _omieClientes;
    private readonly IMediator _mediator;

    public NovoAutoCadastro(IAutoCadastroRepositorio autoCadastroRepositorio, ILogger<NovoAutoCadastro> logger, IUsuarioRepositorio usuarioRepositorio, IClienteRepositorio clienteRepositorio, IOmieClientes omieClientes, IMediator mediator)
    {
        _autoCadastroRepositorio = autoCadastroRepositorio;
        _logger = logger;
        _usuarioRepositorio = usuarioRepositorio;
        _clienteRepositorio = clienteRepositorio;
        _omieClientes = omieClientes;
        _mediator = mediator;
    }

    public async Task<Resposta> Handle(NovoAutoCadastroComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(@"
        **********Processo de Auto Cadastro iniciado.**********");
        #region validacoes
        if (request is null)
        {
            _logger.LogError(@"
        **********Request não pode ser nulo.**********");
            return new("Request não pode ser nulo.", false);
        }

        var existeUsuario = await _usuarioRepositorio.ExisteEmail(request.Cliente.Email);
        if (existeUsuario)
        {
            _logger.LogError(@"
        **********Já existe um usuario cadastrado com esse e-mail.**********");
            return new("Já existe um usuario cadastrado com esse e-mail!", false);
        }

        var existeCliente = await _clienteRepositorio.DocumentoExiste(request.Cliente.Documento);
        if (existeCliente)
        {
            _logger.LogError(@"
        **********Já existe um cliente cadastrado com esse documento.**********");
            return new("Já existe um usuario cadastrado com esse documento!", false);
        }

        var validacao = new AutoCadastroValidacao();
        var resultadoValidacao = validacao.Validate(request);
        if (!resultadoValidacao.IsValid)
        {
            _logger.LogError(@"
        **********Processo de Auto Cadastro falhou devido a erros de validação.**********");

            foreach (var erro in resultadoValidacao.Errors)
            {
                return new(erro.ErrorMessage, false);
            }
        }
        #endregion

        #region criando omie

        EnderecoEntregaOmie enderecoEntrega = new("", "", "", "", "", "");
        List<EnderecoEntregaOmie> omieEnderecoEntrega = new List<EnderecoEntregaOmie>();

        var omieCliente = new OmieCriarClienteRequest(
                    codigo_cliente_integracao: request.Cliente.Documento,
                    email: request.Cliente.Email,
                    razao_social: request.Cliente.Nome,
                    cnpj_cpf: request.Cliente.Documento,
                    telefone1_numero: request.Cliente.Telefone,
                    endereco: request.Cliente.EnderecoCobranca.Logradouro,
                    endereco_numero: request.Cliente.EnderecoCobranca.Numero,
                    bairro: request.Cliente.EnderecoCobranca.Bairro,
                    estado: request.Cliente.EnderecoCobranca.UF,
                    cidade: request.Cliente.EnderecoCobranca.Cidade,
                    cep: request.Cliente.EnderecoCobranca.CEP,
                    contribuinte: request.Cliente.Contribuinte,
                    pessoa_fisica: request.Cliente.Pessoa_fisica,
                    enderecoEntrega: omieEnderecoEntrega
                );


        var omieResponse = await _mediator.Send(omieCliente);
        if (!omieResponse.Sucesso)
        {
            _logger.LogError(@"
            **********Processo de cadastro do cliente na Omie falhou. Erro: {0}**********", new { omieResponse });
            return omieResponse;
        }
        #endregion

        #region criando cliente local
        List<EnderecoEntrega> enderecosEntrega = new List<EnderecoEntrega>();

        EnderecoCobranca enderecoCobranca = new(
                    request.Cliente.EnderecoCobranca.CEP,
                    request.Cliente.EnderecoCobranca.Logradouro,
                    request.Cliente.EnderecoCobranca.Numero,
                    request.Cliente.EnderecoCobranca.Bairro,
                    request.Cliente.EnderecoCobranca.Cidade,
                    request.Cliente.EnderecoCobranca.UF,
                    request.Cliente.EnderecoCobranca.Complemento);

        Cliente cliente = new(request.Cliente.Nome,
                    request.Cliente.Documento,
                    request.Cliente.TipoDocumento,
                    request.Cliente.DataNascimento,
                    request.Cliente.Telefone,
                    request.Cliente.Email,
                    request.Cliente.Contribuinte,
                    enderecoCobranca,
                    enderecosEntrega);


        Int64.TryParse(omieResponse.Mensagem, out long omieId);
        cliente.AtualizarIdOmie(omieId);
        await _clienteRepositorio.IncluirCliente(cliente);
        #endregion

        #region criando usuario local
        Usuario usuario = new(request.Cliente.Nome, request.Cliente.Email, Funcao.Cliente, request.Senha);
        await _usuarioRepositorio.IncluirUsuario(usuario);
        #endregion

        _logger.LogInformation(@"
        **********Processo de Auto Cadastro concluído com sucesso.**********");
        return omieResponse;
    }
}
