namespace Domain.Omie.Cacambas.OmieCacambaResults;

public record OmieObterCacambaResult(IntListar intListar, Cabecalho cabecalho);

public record IntListar(string cCodIntServ, string nCodServ);
public record Cabecalho(
    string cCodigo,
    string cDescricao,
    decimal nPrecoUnit,
    string cCodServMun,
    string cCodLC116,
    string cIdTrib);