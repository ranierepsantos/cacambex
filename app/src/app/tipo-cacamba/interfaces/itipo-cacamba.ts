
export interface IPrecoFaixaCep {
  id: number;
  cepInicial: string;
  cepFinal: string;
  preco: any;
}

export interface VisualizarTipoCacamba {
  id: number;
  volume: string;
  preco: number;
}

export interface AlterarTipoCacamba {
  id: number;
  volume: string;
  preco: any;
  precoFaixaCep: IPrecoFaixaCep[];
}

export interface ITipoCacambaComPrecoFaixaCep {
  id: number;
  volume: string;
  preco: number;
  precoFaixaCep: IPrecoFaixaCep[];
}