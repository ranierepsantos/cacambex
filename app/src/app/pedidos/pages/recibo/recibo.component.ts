import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { VisualizarPedido } from '../../interfaces/ipedido';

interface ReciboItem {
  quantidade: number,
  descricao: string,
  preco: number,
  total: number
}

@Component({
  selector: 'ca-recibo',
  templateUrl: './recibo.component.html',
  styleUrls: ['./recibo.component.css']
})
export class ReciboComponent implements OnInit {

  pedido: VisualizarPedido = {} as VisualizarPedido;
  titulo: string = 'Recibo';
  dataSource = new MatTableDataSource<ReciboItem>([]);
  displayedColumns = ['Quant.', 'Descrição','Vl. Unitário', 'Total'];
  
  constructor(
    private dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: VisualizarPedido,
  ) { }

  ngOnInit(): void {
    if (this.data) {
      this.titulo = 'Recibo nº ' + this.data.id;
      this.pedido = this.data;
      this.dataSource.data = [
        {quantidade: 1, descricao: 'Caçamba ' + this.pedido.pedidoItem.cacamba.volume, preco: this.pedido.valorPedido, total: this.pedido.valorPedido}
      ]
    }
  }

  imprimir() {
    window.print()
    // let area = document.getElementById("print")?.innerHTML ?? ''
    // let printwin = window.open("");
    // printwin?.document.write(area);
    // printwin?.print();
  }
}
