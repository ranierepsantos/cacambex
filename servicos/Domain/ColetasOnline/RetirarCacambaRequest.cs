namespace Domain.ColetasOnline;

public record RetirarCacambaRequest(int iCodCidade,
                                    string stNumeroCTR,
                                    string stDataRetirada,
                                    string stPlacaVeiculo,
                                    int idDestino,
                                    int CTR_Id,
                                    int RecolherItem_Id,
                                    int PedidoConcluido_Id,
                                    int Cacamba_Id);