using Domain.Omie.Clientes.OmieClienteRequests;
using Domain.Clientes.Agrecacao;
using Domain.Clientes.Interface;
using Domain.Clientes.Validacoes;
using Domain.Compartilhado;
using Domain.Omie.Clientes.OmieClienteResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Clientes.Comandos;

public record AtualizarClienteComando(
    int Id,
    string Nome,
    string Email,
    string Telefone,
    DateTime DataNascimento,
    string Contribuinte,
    AlterarEnderecoCobranca EnderecoCobranca,
List<AlterarEnderecoEntrega> EnderecosEntrega) : IRequest<Resposta>;

public record AlterarEnderecoCobranca(
    int Id,
    string CEP,
    string Logradouro,
    string Numero,
    string Complemento,
    string Bairro,
    string Cidade,
    string UF);
public record AlterarEnderecoEntrega(
    int Id,
    string CEP,
    string Logradouro,
    string Numero,
    string Complemento,
    string Bairro,
    string Cidade,
    string UF);
public class AtualizarCliente : IRequestHandler<AtualizarClienteComando, Resposta>
{
    private readonly IClienteRepositorio _repositorio;
    private readonly IMediator _mediator;
    private readonly ILogger<AtualizarCliente> _logger;

    public AtualizarCliente(IClienteRepositorio repositorio, ILogger<AtualizarCliente> logger, IMediator mediator)
    {
        _repositorio = repositorio;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<Resposta> Handle(AtualizarClienteComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(@"
        **********Iniciando processo de atualizacao de cliente**********");

        #region validacoes
        var validacao = new AtualizarClienteValidacao();
        var resultadoValidacao = validacao.Validate(request);
        if (!resultadoValidacao.IsValid)
        {
            _logger.LogError(@"
        **********Processo de atualizacao de cliente falhou devido a erros de validação**********");
            return new("", false, resultadoValidacao.Errors.Select(x => x.ErrorMessage));
        }

        var cliente = _repositorio.ObterClientePorIdComEndereco(request.Id);
        if (cliente == null)
        {
            _logger.LogError(@"
        **********Cliente não encontrado.**********");
            return new("Cliente não encontrado.", false);
        }
        #endregion

        #region omie
        var omieEndereco = request.EnderecosEntrega
                            .Select(x =>
                            new EnderecoEntregaOmie(
                                x.Logradouro,
                                x.Numero,
                                x.Bairro,
                                x.CEP,
                                x.UF,
                                x.Cidade
                            )).ToList();

        OmieAlterarClienteRequest omieRequest = new(
            codigo_cliente_integracao: cliente.Codigo_cliente_integracao,
            email: request.Email,
            razao_social: request.Nome,
            cnpj_cpf: cliente.Documento,
            telefone1_numero: request.Telefone,
            endereco: request.EnderecoCobranca.Logradouro,
            endereco_numero: request.EnderecoCobranca.Numero,
            bairro: request.EnderecoCobranca.Bairro,
            estado: request.EnderecoCobranca.UF,
            cidade: request.EnderecoCobranca.Cidade,
            cep: request.EnderecoCobranca.CEP,
            contribuinte: request.Contribuinte,
            pessoa_fisica: cliente.Pessoa_fisica,
            enderecoEntrega: omieEndereco
        );

        var omieResponse = await _mediator.Send(omieRequest);
        if (!omieResponse.Sucesso)
        {
            _logger.LogError(@"
            **********Processo de atualizacao do cliente na Omie falhou. Erro: {0}**********", new { omieResponse });
            return omieResponse;
        }

        #endregion

        #region local

        List<EnderecoEntrega> enderecosEntregaParaAdicionar = request
                        .EnderecosEntrega
                        .Where(x => x.Id == 0)
                        .Select(x => new EnderecoEntrega(
                            x.CEP,
                            x.Logradouro,
                            x.Numero,
                            x.Bairro,
                            x.Cidade,
                            x.UF,
                            x.Complemento))
                        .ToList();

        foreach (EnderecoEntrega novoEnderecoEntrega in enderecosEntregaParaAdicionar)
        {
            cliente.AdicionarEnderecoEntrega(novoEnderecoEntrega);
        }

        List<EnderecoEntrega> enderecosParaRemover = cliente
           .EnderecosEntrega
           .Where(x => !request.EnderecosEntrega.Any(y => y.Id == x.Id))
                .Where(x => x.Id != 0)
                .ToList();

        foreach (var removerEnd in enderecosParaRemover)
        {
            cliente.RemoverEnderecoEntrega(removerEnd);
        }


        List<EnderecoEntrega> enderecosParaAtualizar = cliente
            .EnderecosEntrega
            .Where(x => request.EnderecosEntrega.Select(x => x.Id).Contains(x.Id))
            .Where(x => x.Id != 0)
             .ToList();

        enderecosParaAtualizar.ForEach((endereco) =>
           {
               var alteracoes = request.EnderecosEntrega.FirstOrDefault(y => y.Id == endereco.Id);
               endereco.AtualizarEnderecoEntrega(
               alteracoes.CEP,
               alteracoes.Logradouro,
               alteracoes.Numero,
               alteracoes.Complemento,
               alteracoes.Bairro,
               alteracoes.Cidade,
               alteracoes.UF);
               cliente.AlterarEnderecoEntrega(endereco);
           });


        EnderecoCobranca enderecoCobranca = new(
            request.EnderecoCobranca.CEP,
            request.EnderecoCobranca.Logradouro,
            request.EnderecoCobranca.Numero,
            request.EnderecoCobranca.Bairro,
            request.EnderecoCobranca.Cidade,
            request.EnderecoCobranca.UF,
            request.EnderecoCobranca.Complemento
        );
        cliente.AlterarCliente(request.Nome, request.Email, request.Telefone, request.DataNascimento, request.Contribuinte, enderecoCobranca);
        await _repositorio.AtualizarCliente(cliente);

        #endregion
        _logger.LogInformation(@"
        **********Cliente atualizado com sucesso.**********");

        return omieResponse;
    }
}