using System;
using System.Collections.Generic;
using Domain.Clientes.Agrecacao;
using Domain.Clientes.Enumeraveis;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Entidades;

public class ClienteTests
{
    List<EnderecoEntrega> _enderecosEntrega;

    public ClienteTests()
    {
        _enderecosEntrega = new List<EnderecoEntrega>();
        EnderecoEntrega enderecoEntrega = new(
            cep: "13219110",
            logradouro: "rua teste",
            numero: "1",
            complemento: "casa verde",
            bairro: "teste",
            cidade: "Jundiaí",
            uf: "sp"
        );
        _enderecosEntrega.Add(enderecoEntrega);
    }

    [Fact]
    public void FornecidoOsDadosCorretos_AoExecutar_Sucesso()
    {
        Cliente cliente = new(
            nome: "José",
            documento: "11122233344",
            tipoDocumento: TipoDocumento.Cpf,
            dataNascimento: DateTime.Now.AddYears(-18),
            telefone: "11910102020",
            email: "jose@jose.com",
            contribuinte: "s",
            enderecoCobranca: new EnderecoCobranca(
                cep: "13219110",
                logradouro: "rua teste",
                numero: "1",
                complemento: "casa verde",
                bairro: "teste",
                cidade: "Jundiaí",
                uf: "sp"
            ),
            enderecosEntrega: _enderecosEntrega
        );
        Assert.NotNull(cliente.Documento);
        Assert.NotEmpty(cliente.Documento);
    }
}