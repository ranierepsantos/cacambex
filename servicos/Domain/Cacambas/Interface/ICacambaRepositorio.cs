using Domain.Cacambas.Agregacao;

namespace Domain.Cacambas.Interface;

public interface ICacambaRepositorio
{
    Task IncluirCacamba(Cacamba cacamba);
    Task AtualizarCacamba(Cacamba cacamba);
    Task DeletarCacamba(Cacamba cacamba);
    Cacamba? ObterPorId(int id);
    Cacamba? ObterPorNumero(string numero);
    Cacamba? ObterPorVolume(string volume);
    Task<bool> CacambaExiste(string numero);

}