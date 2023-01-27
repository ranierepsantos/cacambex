import { Component, Input, OnInit } from '@angular/core';
import { Evento } from 'src/app/pedidos/interfaces/ieventos';
import { VisualizarPedido } from 'src/app/pedidos/interfaces/ipedido';


@Component({
  selector: 'ca-acompanhamento-de-pedido',
  templateUrl: './acompanhamento-de-pedido.component.html',
  styleUrls: ['./acompanhamento-de-pedido.component.css']
})
export class AcompanhamentoDePedidoComponent implements OnInit {
  @Input() infoPedido!: VisualizarPedido;
  @Input() fasesDoPedido!: Evento;

  constructor() { }

  ngOnInit(): void {
  }
  concatenandoEndereco() {
    return `${this.infoPedido.enderecoEntrega.logradouro},
    ${this.infoPedido.enderecoEntrega.numero} -
    ${this.infoPedido.enderecoEntrega.bairro} -
    ${this.infoPedido.enderecoEntrega.cidade} -
    ${this.infoPedido.enderecoEntrega.uf} -
    ${this.infoPedido.enderecoEntrega.cep}.`;
  }
}
