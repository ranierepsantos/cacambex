import { Component, Input, OnInit } from '@angular/core';
import { VisualizarEvento } from 'src/app/pedidos/interfaces/ieventos';

@Component({
  selector: 'ca-caixa-status',
  templateUrl: './caixa-status.component.html',
  styleUrls: ['./caixa-status.component.css']
})
export class CaixaStatusComponent implements OnInit {
  @Input() visualizarEvento!: VisualizarEvento;

  constructor() { }

  ngOnInit(): void {
  }

}
