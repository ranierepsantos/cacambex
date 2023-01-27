import { TipoDocumento } from "../enum/tipoDocumento";
import { AlterarEndereco, NovoEndereco, VisualizarEndereco } from "./ienderecos";

export interface NovoCliente {
  nome: string;
  documento: string;
  tipoDocumento: TipoDocumento;
  dataNascimento: Date;
  telefone: string;
  email: string;
  contribuinte: string;
  enderecoCobranca: NovoEndereco;
  enderecosEntrega: NovoEndereco[];
}
export interface VisualizarCliente {
  id: number;
  nome: string;
  documento: string;
  tipoDocumento: TipoDocumento;
  dataNascimento: Date;
  telefone: string;
  email: string;
  contribuinte: string;
  enderecoCobranca: VisualizarEndereco;
  enderecosEntrega: VisualizarEndereco[];
}
export interface AlterarCliente {
  id: number;
  nome: string;
  documento: string;
  tipoDocumento: TipoDocumento;
  dataNascimento: Date;
  telefone: string;
  email: string;
  contribuinte: string;
  endereco: AlterarEndereco;
  enderecos: AlterarEndereco[];
}
