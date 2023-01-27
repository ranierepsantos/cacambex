namespace CTRs.Models.EnviarCacambaObra;

public record EnviarCacambaObraRequest(int iCodCidade,
                                       string stNumeroCTR,
                                       string stDataEnvio,
                                       string stPlacaVeiculo,
                                       string stIDentificacaoCacamba,
                                       string stLocalEstacionamento,
                                       int CTR_Id,
                                       int RecolherItem_Id);
