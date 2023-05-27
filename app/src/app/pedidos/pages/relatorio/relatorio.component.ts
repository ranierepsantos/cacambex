import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { VisualizarPedido } from '../../interfaces/ipedido';
import { PedidoService } from '../../servicos/pedido.service';
import { jsPDF } from "jspdf"

@Component({
  selector: 'ca-relatorio',
  templateUrl: './relatorio.component.html',
  styleUrls: ['./relatorio.component.css']
})
export class relatorioComponent implements OnInit {

  @ViewChild("print",{static: false}) el!: ElementRef;

  pedidos: VisualizarPedido[] = [];
  titulo: string = 'Relatório de Pedidos';
  dataSource = new MatTableDataSource<VisualizarPedido>([]);
  displayedColumns = ['Data', 'Cliente','Caçamba','Valor Pedido'];
  
  constructor(
    private pedidoService: PedidoService
  ) { }

  ngOnInit(): void {
      //this.titulo = this.titulo.replace("{dataIni}", "01/01/2023").replace("{dataFim}", "24/05/2023")
      this.dataSource.data = this.pedidoService.getPedidos();
  }

  imprimir() {
    let doc = new jsPDF('l', 'mm', [1500,1500])
    doc.html(this.el.nativeElement, {callback: (doc) => {
      doc.save("relatorioPedidos.pdf")
    }});
    //doc.text('Hello world!', 10, 10)
    //doc.save('a4.pdf')
  }
}
