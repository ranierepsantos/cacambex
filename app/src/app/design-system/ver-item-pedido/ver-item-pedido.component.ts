import { VisualizarCacamba } from 'src/app/cacambas/interfaces/icacamba';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'ca-ver-item-pedido',
  templateUrl: './ver-item-pedido.component.html',
  styleUrls: ['./ver-item-pedido.component.css']
})
export class VerItemPedidoComponent implements OnInit {
  @Input() cacamba!: VisualizarCacamba;
  constructor() { }

  ngOnInit(): void {
  }

}
