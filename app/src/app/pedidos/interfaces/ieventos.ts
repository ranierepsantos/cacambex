import { StatusPedido } from "../enums/status-pedido";


export interface VisualizarEvento {
  quando: string;
  descricao: string;
  status: StatusPedido;
  mensagem: string;
}

export interface Evento {
  id: number;
  quando: Date;
  descricao: string;
  status: StatusPedido;
  mensagem: string;
}
export interface CTR extends Evento { }
export interface Recolher extends Evento { }
export interface Entregue extends Evento { }
export interface Concluido extends Evento { }
