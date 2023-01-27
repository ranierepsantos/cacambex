export interface SolicitarCTR {
  pedidoId: number;
  classificacao: string;
  classeResiduo: string;
}
export interface EnviarCacambaObra {
  pedidoId: number;
  placaVeiculo: string;
  numeroCacamba: string;
  localEstacionado: string;
}
export interface RecolherCacambaObra {
  pedidoId: number;
  placaVeiculo: string;
}
