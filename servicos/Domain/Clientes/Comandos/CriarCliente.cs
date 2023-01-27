using System.Text.Json;
using Domain.Omie.Clientes.OmieClienteRequests;
using Domain.Clientes.Agrecacao;
using Domain.Clientes.Enumeraveis;
using Domain.Clientes.Interface;
using Domain.Clientes.Validacoes;
using Domain.Compartilhado;
using Domain.Omie.Clientes.OmieClienteResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Clientes.Comandos;

public record CriarClienteComando(
    string Nome,
    string Documento,
    TipoDocumento TipoDocumento,
    DateTime DataNascimento,
    string Telefone,
    string Email,
    string Contribuinte,
    NovoEnderecoCobranca EnderecoCobranca,
    List<NovoEnderecoEntrega> EnderecosEntrega) : IRequest<Resposta>;

public record NovoEnderecoCobranca(
    string CEP,
    string Logradouro,
    string Numero,
    string Complemento,
    string Bairro,
    string Cidade,
    string UF);
public record NovoEnderecoEntrega(
    string CEP,
    string Logradouro,
    string Numero,
    string Complemento,
    string Bairro,
    string Cidade,
    string UF);
public class CriarClienteManipulador : IRequestHandler<CriarClienteComando, Resposta>
{
    private readonly IMediator _mediator;
    private readonly ILogger<CriarClienteManipulador> _logger;
    private readonly IClienteRepositorio _clienteRepositorio;
    public CriarClienteManipulador(IClienteRepositorio repositorio, ILogger<CriarClienteManipulador> logger, IMediator mediator)
    {
        _clienteRepositorio = repositorio;
        _logger = logger;
        _mediator = mediator;
    }
    public async Task<Resposta> Handle(CriarClienteComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(@"
        **********Processo de criação de um novo cliente iniciado**********");

        #region validacoes
        var validacao = new CriarClienteValidacao();
        var resultadoValidacao = validacao.Validate(request);
        if (!resultadoValidacao.IsValid)
        {
            _logger.LogError(@"
        **********Processo de criacao de cliente falhou devido a erros de validação**********");
            return new("", false, resultadoValidacao.Errors.Select(x => x.ErrorMessage));
        }

        var existeDocumento = await _clienteRepositorio.DocumentoExiste(request.Documento);
        if (existeDocumento)
        {
            _logger.LogInformation(@"
        **********Já existe um cliente cadastrado com esse documento**********");
            return new("Já existe um cliente cadastrado com esse documento.", false);
        }
        #endregion

        var omieEnderecos = request.EnderecosEntrega
                        .Select(x => new EnderecoEntregaOmie(
                            x.Logradouro,
                            x.Numero,
                            x.Bairro,
                            x.CEP,
                            x.UF,
                            x.Cidade
                            )
                            ).ToList();

        #region obtendo cliente p/ omie
        var enderecosEntrega = request.EnderecosEntrega
                                .Select(x => new EnderecoEntrega(
                                    x.CEP,
                                    x.Logradouro,
                                    x.Numero,
                                    x.Bairro,
                                    x.Cidade,
                                    x.UF,
                                    x.Complemento)
                                    ).ToList();

        EnderecoCobranca enderecoCobranca = new(
                    request.EnderecoCobranca.CEP,
                    request.EnderecoCobranca.Logradouro,
                    request.EnderecoCobranca.Numero,
                    request.EnderecoCobranca.Bairro,
                    request.EnderecoCobranca.Cidade,
                    request.EnderecoCobranca.UF,
                    request.EnderecoCobranca.Complemento);

        var cliente = new Cliente(request.Nome,
                                  request.Documento,
                                  request.TipoDocumento,
                                  request.DataNascimento,
                                  request.Telefone,
                                  request.Email,
                                  request.Contribuinte,
                                  enderecoCobranca,
                                  enderecosEntrega);

        if (cliente.Pessoa_fisica is null)
        {
            _logger.LogInformation(@"
        **********Cliente.Pessoa_Fisica não pode ser nulo**********");
            return new("Cliente.Pessoa_Fisica não pode ser nulo.", false);
        }
        var omieCliente = new OmieCriarClienteRequest(
            codigo_cliente_integracao: request.Documento,
            email: request.Email,
            razao_social: request.Nome,
            cnpj_cpf: request.Documento,
            telefone1_numero: request.Telefone,
            endereco: request.EnderecoCobranca.Logradouro,
            endereco_numero: request.EnderecoCobranca.Numero,
            bairro: request.EnderecoCobranca.Bairro,
            estado: request.EnderecoCobranca.UF,
            cidade: request.EnderecoCobranca.Cidade,
            cep: request.EnderecoCobranca.CEP,
            contribuinte: request.Contribuinte,
            pessoa_fisica: cliente.Pessoa_fisica,
            enderecoEntrega: omieEnderecos
        );
        _logger.LogInformation(@"
            **********Cadastrando cliente na Omie...**********");
        var omieResponse = await _mediator.Send(omieCliente);
        if (!omieResponse.Sucesso)
        {
            _logger.LogError(@"
            **********Processo de cadastro do cliente na Omie falhou. Erro: {0}**********", new { omieResponse });
            return omieResponse;
        }
        #endregion

        #region salvando local
        Int64.TryParse(omieResponse.Mensagem, out long omieId);
        cliente.AtualizarIdOmie(omieId);
        await _clienteRepositorio.IncluirCliente(cliente);
        #endregion

        _logger.LogInformation(@"
            **********Cliente cadastrado com sucesso**********");
        return omieResponse;
    }
}