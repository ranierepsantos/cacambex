import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { VisualizarEvento } from 'src/app/pedidos/interfaces/ieventos';
import { VisualizarPedido } from 'src/app/pedidos/interfaces/ipedido';
import { VisualizarPedidoItem } from 'src/app/pedidos/interfaces/ipedidoItem';

@Component({
  selector: 'ca-nota-fiscal',
  templateUrl: './nota-fiscal.component.html',
  styleUrls: ['./nota-fiscal.component.css']
})
export class NotaFiscalComponent implements OnInit {
  @Input() pedidoItem$!: BehaviorSubject<VisualizarPedidoItem>;
  @Input() notaFiscal$!: BehaviorSubject<VisualizarEvento>;
  @Input() pedido$!: BehaviorSubject<VisualizarPedido>;
  loading = false;
  @Input() set enviando(value: boolean) {
    this.loading = value;
  };
  @Output() emitirNfe: EventEmitter<void> = new EventEmitter();
  @Output() atualizarStatusPedido: EventEmitter<void> = new EventEmitter();


  constructor() { }

  ngOnInit(): void {
  }
}
