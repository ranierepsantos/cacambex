import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BehaviorSubject } from 'rxjs';
import { SnackResponseService } from 'src/app/design-system/snack-response.service';

import { EnviarCacambaObra, RecolherCacambaObra, SolicitarCTR } from '../../interfaces/ictr';
import { VisualizarEvento } from '../../interfaces/ieventos';
import { VisualizarPedido } from '../../interfaces/ipedido';
import { VisualizarPedidoItem } from '../../interfaces/ipedidoItem';
import { PedidoService } from '../../servicos/pedido.service';

@Component({
  selector: 'ca-emitir-ctr',
  templateUrl: './emitir-ctr.component.html',
  styleUrls: ['./emitir-ctr.component.css']
})
export class EmitirCtrComponent implements OnInit {
  solicitarCtr = {} as SolicitarCTR;
  enviarCacambaObra = {} as EnviarCacambaObra;
  recolherCacambaObra = {} as RecolherCacambaObra;
  pedidoItem = {} as VisualizarPedidoItem;
  visualizarPedido$ = new BehaviorSubject<VisualizarPedido>({} as VisualizarPedido);
  visualizarRecolher$ = new BehaviorSubject<VisualizarEvento>({} as VisualizarEvento);

  constructor(
    @Inject(MAT_DIALOG_DATA) public pedido: VisualizarPedido,
    private pedidoService: PedidoService,
    private snackBar: SnackResponseService,
    public dialogRef: MatDialogRef<EmitirCtrComponent>) { }

  ngOnInit(): void {
    this.visualizarPedido$.next(this.pedido);
    this.pedidoItem = this.pedido.pedidoItem;
    setTimeout(() => {
      this.obterEventoRecolher(this.visualizarPedido$.value.eventos);
    }, 100);
  }

  solicitarCTR() {
    this.solicitarCtr.pedidoId = this.pedido.id;
    this.pedidoService.solicitarCTR(this.pedido.id, this.solicitarCtr).subscribe((x) => {
      this.dialogRef.close();
      this.snackBar.mostrarMensagem("CTR solicitado com sucesso!");
    }, (e) => {
      this.dialogRef.close();
      this.snackBar.mostrarMensagem(e.error.mensagem, true);
    })
  }
  enviarCacamba() {
    this.enviarCacambaObra.pedidoId = this.pedido.id;
    this.enviarCacambaObra.numeroCacamba = this.pedidoItem.cacamba.numero;
    this.pedidoService.enviarCacamba(this.pedido.id, this.enviarCacambaObra).subscribe((x) => {
      this.dialogRef.close();
      this.snackBar.mostrarMensagem("Envio de caçamba solicitado com sucesso!");
    }, (e) => {
      this.dialogRef.close();
      this.snackBar.mostrarMensagem(e.error.mensagem, true);
    })
  }
  recolherCacamba() {
    this.recolherCacambaObra.pedidoId = this.pedido.id;
    this.pedidoService.recolherCacamba(this.pedido.id, this.recolherCacambaObra).subscribe((x) => {

      this.dialogRef.close();
      this.snackBar.mostrarMensagem("Solicitação para recolher caçamba enviado com sucesso!");
    }, (e) => {
      this.dialogRef.close();
      this.snackBar.mostrarMensagem(e.error.mensagem, true);
    })
  }
  private obterEventoRecolher(eventos: VisualizarEvento[]) {
    let eventoRecolher = eventos.filter((x) => x.descricao == 'Recolher')[0];
    this.visualizarRecolher$.next(eventoRecolher);
  }
}
