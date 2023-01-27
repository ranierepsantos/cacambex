using Domain.Compartilhado;

namespace Domain.ViaCep.Interface;
public interface IBuscarCep
{
    Task<BuscarCepResult> BuscarEndereco(string cep);
}