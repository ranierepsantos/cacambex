namespace Domain.Omie.Clientes.OmieClienteResults;
public record OmieOperacaoSucessoClienteResult(long codigo_cliente_omie, string codigo_cliente_integracao, string codigo_status, string descricao_status);
public record OmieCriarClienteResult(long codigo_cliente_omie);