using Domain.AzureStorage;
using Domain.ColetasOnline;
using Domain.Compartilhado;
using Domain.Pedidos.Interface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Pedidos.Comandos;

public record SolicitaCtrComando(int PedidoId,
                             int Classificacao,
                             int ClasseResiduo
                             ) : IRequest<Resposta>;

public class SolicitaCtrHandler : IRequestHandler<SolicitaCtrComando, Resposta>
{
    private readonly IPedidoRepositorio _pedidoRepositorio;
    private readonly ILogger<SolicitaCtrHandler> _logger;
    private readonly IFilaSolicitaCacambaRepositorio _queueRepositorio;

    public SolicitaCtrHandler(IPedidoRepositorio pedidoRepositorio,
                        IFilaSolicitaCacambaRepositorio queueRepositorio,
                        ILogger<SolicitaCtrHandler> logger)
    {
        _pedidoRepositorio = pedidoRepositorio;
        _queueRepositorio = queueRepositorio;
        _logger = logger;
    }

    public async Task<Resposta> Handle(SolicitaCtrComando request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("**********Processo de solicitacao de CTR iniciado.**********");

        var pedido = await _pedidoRepositorio.ObterPedidoPorIdAsync(request.PedidoId);
        if (pedido is null)
        {
            _logger.LogError("**********Pedido nao encontrado.**********");
            return new Resposta("Pedido nao encontrado.", false);
        }

        var tipoVeiculo = (pedido.PedidoItem.Cacamba?.Volume) switch
        {
            "3M³" => 19,
            "5M³" => 20,
            _ => 0
        };

        var volumeCacamba = (pedido.PedidoItem.Cacamba?.Volume) switch
        {
            "3M³" => 3,
            "5M³" => 5,
            _ => 0
        };

        SolicitaCTRRequest ctrRequest = new(
            iCodCidade: 25,
            iTipoVeiculo: tipoVeiculo,
            iClassificacao: request.Classificacao,
            iClasse: request.ClasseResiduo,
            ivolume: volumeCacamba,
            iGGTipo: 1,
            GG_CPF: pedido.Cliente.Documento,
            GG_Nome: pedido.Cliente.Nome,
            GG_Email: pedido.Cliente.Email,
            GG_Endereco_CEP: pedido.Cliente.EnderecoCobranca.CEP,
            GG_Endereco_Rua: pedido.Cliente.EnderecoCobranca.Logradouro,
            GG_Endereco_Num: pedido.Cliente.EnderecoCobranca.Numero,
            GG_Endereco_Compl: pedido.Cliente.EnderecoCobranca.Complemento,
            GG_Endereco_Bairro: pedido.Cliente.EnderecoCobranca.Bairro,
            GG_Endereco_Cidade: pedido.Cliente.EnderecoCobranca.Cidade,
            CTR_CEP: pedido.EnderecoEntrega.CEP,
            CTR_Rua: pedido.EnderecoEntrega.Logradouro,
            CTR_Num: pedido.EnderecoEntrega.Numero,
            CTR_Compl: pedido.EnderecoEntrega.Complemento,
            CTR_Bairro: pedido.EnderecoEntrega.Bairro,
            CTR_Cidade: pedido.EnderecoEntrega.Cidade,
            CTR_Id: pedido.PedidoItem.CTR.Id,
            PedidoId: request.PedidoId
        );

        //pedido.PedidoItem.AtualizarCTR("Solicitacao de CTR enviada para fila de processamento.");
        try
        {
            await _pedidoRepositorio.AtualizarPedidoAsync(pedido);
            await _queueRepositorio.FilaSolicitaCTR(ctrRequest);
            _logger.LogInformation("**********Processo de solicitacao de CTR concluído com sucesso.**********");

        }
        catch (Exception ex)
        {
            _logger.LogError(@$"**********ERRO ao enviar requisicao para fila de processamento ou ao atualizar pedido no banco: 
                                Exception: {ex.Message}**********");
            throw;
        }

        return new("Solicitacao de CTR enviada para fila de processamento.");
    }
}
