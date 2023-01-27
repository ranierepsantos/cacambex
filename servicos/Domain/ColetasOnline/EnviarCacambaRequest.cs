namespace Domain.ColetasOnline;
public record EnviarCacambaRequest(int iCodCidade,
                                   string stNumeroCTR,
                                   string stDataEnvio,
                                   string stPlacaVeiculo,
                                   string stIDentificacaoCacamba,
                                   string stLocalEstacionamento,
                                   int RecolherItem_Id);