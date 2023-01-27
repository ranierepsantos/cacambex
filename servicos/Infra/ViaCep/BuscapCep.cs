using Domain.ViaCep.Interface;
using Flurl.Http;

namespace Infra.ViaCep;
public class BuscarCep : IBuscarCep
{
    private readonly string _viaCepUrl = "https://viacep.com.br/ws";
    public async Task<BuscarCepResult> BuscarEndereco(string cep)
    {
        var httpResult = await $"{_viaCepUrl}/{cep}/json"
        .GetJsonAsync<BuscarCepResult>();
        return httpResult;
    }
}
