namespace Domain.Compartilhado;

public record Resposta(string Mensagem, bool Sucesso = true, object? Dados = null);


