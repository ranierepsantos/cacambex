namespace Domain.Omie.Pedidos;
public record OmieOrdemServicoResult(string cCodIntOS,
                                     long nCodOS,
                                     string cNumOS,
                                     string cCodStatus,
                                     string cDescStatus);
public record OmieFaturarOSResult(string cCodIntOS,
                                  long nCodOS,
                                  string cCodStatus,
                                  string cDescStatus);

public record OmieConsultarStatusPedidoResult(List<ListaRpsNfse> ListaRpsNfse);
public record ListaRpsNfse(List<Mensagens> Mensagens, string nNfse);
public record Mensagens(string cCodigo, string cCorrecao, string cDescricao);