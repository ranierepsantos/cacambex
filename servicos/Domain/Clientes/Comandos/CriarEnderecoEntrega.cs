using Domain.Clientes.Agrecacao;
using Domain.Clientes.Interface;
using Domain.Clientes.Validacoes;
using Domain.Compartilhado;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Clientes.Comandos;
public record CriarEnderecoEntregaComClienteIdComando(
     int ClienteId,
    string CEP,
    string Logradouro,
    string Numero,
    string Complemento,
    string Bairro,
    string Cidade,
    string UF
) : IRequest<Resposta>;

public class CriarEnderecoEntregaComClienteIdHandler : IRequestHandler<CriarEnderecoEntregaComClienteIdComando, Resposta>
{
    private readonly IMediator _mediator;
    private readonly IClienteRepositorio _clienteRepositorio;
    private readonly ILogger<CriarEnderecoEntregaComClienteIdHandler> _logger;


    public CriarEnderecoEntregaComClienteIdHandler(IMediator mediator, IClienteRepositorio clienteRepositorio, ILogger<CriarEnderecoEntregaComClienteIdHandler> logger)
    {
        _mediator = mediator;
        _clienteRepositorio = clienteRepositorio;
        _logger = logger;
    }

    public async Task<Resposta> Handle(CriarEnderecoEntregaComClienteIdComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(@"
        **********Processo de criação de um endereço de entrega para o cliente iniciado**********");
        var validacao = new NovoEnderecoValidacao();
        var resultadoValidacao = validacao.Validate(request);
        if (!resultadoValidacao.IsValid)
        {
            _logger.LogError(@"
        **********Processo de criacao de endereco de entrega para um cliente falhou devido a erros de validação**********");
            return new("", false, resultadoValidacao.Errors.Select(x => x.ErrorMessage));

        }

        var cliente = _clienteRepositorio.ObterClientePorIdComEndereco(request.ClienteId);

        if (cliente is null)
        {
            _logger.LogError(@"
            **********Cliente não encontrado.**********");
            return new("Cliente não encontrado.", false);
        }

        cliente.AdicionarEnderecoEntrega(new EnderecoEntrega(
            cep: request.CEP,
            logradouro: request.Logradouro,
            numero: request.Numero,
            bairro: request.Bairro,
            cidade: request.Cidade,
            uf: request.UF,
            complemento: request.Complemento
        ));

        await _clienteRepositorio.AtualizarCliente(cliente);
        _logger.LogInformation(@"
        **********Endereço de entrega cadastrado com sucesso.**********");
        return new("Endereço de entrega cadastrado com sucesso.");
    }
}
