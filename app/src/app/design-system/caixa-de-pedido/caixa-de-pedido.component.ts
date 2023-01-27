import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { VisualizarPedido } from 'src/app/pedidos/interfaces/ipedido';


@Component({
  selector: "ca-caixa-de-pedido",
  templateUrl: "./caixa-de-pedido.component.html",
  styleUrls: ["./caixa-de-pedido.component.css"],
})
export class CaixaDePedidoComponent implements OnInit {
  confirmarExclusao = true;
  @Input() pedidoVisualizar!: VisualizarPedido;
  @Input() isAdmin!: boolean;
  @Output() solicitarColeta: EventEmitter<void> = new EventEmitter();
  @Output() gerenciarPedido: EventEmitter<void> = new EventEmitter();
  @Output() verDetalhes: EventEmitter<void> = new EventEmitter();
  @Output() excluir: EventEmitter<void> = new EventEmitter();
  @Output() editar: EventEmitter<void> = new EventEmitter();
  @Output() notaFiscal: EventEmitter<void> = new EventEmitter();
  @Output() entregue: EventEmitter<void> = new EventEmitter();
  mensagemEditar = 'Nota fiscal já emitida. Não é possível editar o pedido.';
  mensagemExcluir = 'Nota fiscal já emitida. Não é possível excluir o pedido.';
  constructor() { }
  ngOnInit(): void {
  }

  concatenandoEndereco() {
    return `${this.pedidoVisualizar.enderecoEntrega.logradouro},
    ${this.pedidoVisualizar.enderecoEntrega.numero},
    ${this.pedidoVisualizar.enderecoEntrega.bairro},
    ${this.pedidoVisualizar.enderecoEntrega.cep},
    ${this.pedidoVisualizar.enderecoEntrega.cidade},
    ${this.pedidoVisualizar.enderecoEntrega.uf}.`;
  }
}
