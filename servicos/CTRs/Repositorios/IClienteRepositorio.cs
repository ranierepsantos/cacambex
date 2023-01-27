using System.Threading.Tasks;

namespace CTRs.Repositorios;

public interface IClienteRepositorio
{
    Task<bool> SalvarNumeroCTRComSucesso(int id, string mensagem, int pedidoId, string numeroCtr);
    Task<bool> SalvarNumeroCTRComErro(int id, string mensagem);
    Task<bool> SetStatusAguardandoParaRecolherCacamba(int id, string mensagem);
    Task<bool> SetStatusConcluidoParaRecolherCacambaEPedidoConcluido(int id, int pedidoConcluidoId, int cacambaId,
                                                                     string mensagem);


}
