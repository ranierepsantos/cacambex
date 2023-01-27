import { Status } from "../enum/status";

export interface NovaCacamba {
  numero: string;
  volume: string;
  preco: number;
}

export interface VisualizarCacamba {
  id: number;
  numero: string;
  volume: string;
  preco: number;
}

export interface AlterarCacamba {
  id: number;
  numero: string;
  volume: string;
  preco: number;
}
export interface Cacamba {
  id: number;
  numero: string;
  volume: string;
  status: Status;
}
