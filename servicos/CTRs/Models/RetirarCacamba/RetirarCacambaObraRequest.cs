namespace CTRs.Models.RetirarCacamba;

public record RetirarCacambaObraRequest(int iCodCidade,
                                        string stNumeroCTR,
                                        string stDataRetirada,
                                        string stPlacaVeiculo,
                                        int idDestino,
                                        int CTR_Id,
                                        int RecolherItem_Id,
                                        int PedidoConcluido_Id,
                                        int Cacamba_Id);
