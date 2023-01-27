import { VisualizarEndereco } from "src/app/clientes/interfaces/ienderecos";
import { TipoDePagamento } from "src/app/pedidos/enums/tipo-pagamento";
import { VisualizarEvento } from "./ieventos";
import { VisualizarPedidoItem } from "./ipedidoItem";

export interface NovoPedido {
  clienteId: number;
  volumeCacamba: string;
  enderecoId: number;
  tipoDePagamento: TipoDePagamento;
  observacao: string;
}

export interface VisualizarPedido {
  id: number;
  nomeCliente: string;
  idCliente: number;
  documentoCliente: string;
  telefoneCliente: string;
  emailCliente: string;
  numeroNotaFiscal: string;
  numeroCTR: string;
  tipoDePagamento: TipoDePagamento;
  pedidoItem: VisualizarPedidoItem;
  enderecoEntrega: VisualizarEndereco;
  valorPedido: number;
  observacao: string;
  eventos: VisualizarEvento[];
  emitidoEm: Date;
}

export interface AlterarPedido {
  pedidoId: number;
  enderecoId: number;
  volumeCacamba: string;
  tipoDePagamento: TipoDePagamento;
  observacao: string;
}
