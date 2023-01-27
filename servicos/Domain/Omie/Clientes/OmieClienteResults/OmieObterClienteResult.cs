namespace Domain.Omie.Clientes.OmieClienteResults;
public record OmieObterClienteResult(
        string codigo_cliente_integracao,
        string email,
        string razao_social,
        string cnpj_cpf,
        string contato,
        string telefone1_numero,
        string endereco,
        string endereco_numero,
        string bairro,
        string estado,
        string cidade,
        string cep,
        string contribuinte,
        string pessoa_fisica,
        string observacao,
        List<EnderecoEntregaOmie> enderecoEntrega
);
public record EnderecoEntregaOmie
{
    public EnderecoEntregaOmie(string entEndereco, string entNumero, string entBairro, string entCEP, string entEstado, string entCidade)
    {
        this.entEndereco = entEndereco;
        this.entNumero = entNumero;
        this.entBairro = entBairro;
        this.entCEP = entCEP;
        this.entEstado = entEstado.ToUpper();
        this.entCidade = entCidade;
    }

    public string entEndereco { get; init; }
    public string entNumero { get; init; }
    public string entBairro { get; init; }
    public string entCEP { get; init; }
    public string entEstado { get; init; }
    public string entCidade { get; init; }
}



