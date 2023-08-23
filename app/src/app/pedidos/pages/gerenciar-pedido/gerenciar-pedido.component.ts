import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, of } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { PopupInformativoComponent } from 'src/app/design-system/popup-informativo/popup-informativo.component';
import { SnackResponseService } from 'src/app/design-system/snack-response.service';

import { StatusPedido } from '../../enums/status-pedido';
import { VisualizarEvento } from '../../interfaces/ieventos';
import { NotaFiscal, StatusNotaFiscal } from '../../interfaces/inota-fiscal';
import { IStatusPedidoOmie } from '../../interfaces/iomie-response';
import { VisualizarPedido } from '../../interfaces/ipedido';
import { VisualizarPedidoItem } from '../../interfaces/ipedidoItem';
import { VincularCacamba } from '../../interfaces/vincular-cacamba';
import { EmitirCtrComponent } from '../emitir-ctr/emitir-ctr.component';
import { SelecionarCacambaComponent } from '../selecionar-cacamba/selecionar-cacamba.component';
import { VisualizarCacamba } from './../../../cacambas/interfaces/icacamba';
import { PedidoService } from './../../servicos/pedido.service';

@Component({
  templateUrl: "./gerenciar-pedido.component.html",
  styleUrls: ["./gerenciar-pedido.component.css"],
})
export class GerenciarPedidoComponent implements OnInit {
  pedidoItem$ = new BehaviorSubject<VisualizarPedidoItem>({} as VisualizarPedidoItem);
  visualizarPedido$ = new BehaviorSubject<VisualizarPedido>({} as VisualizarPedido);
  visualizarNotaFiscal$ = new BehaviorSubject<VisualizarEvento>({} as VisualizarEvento);
  visualizarCTR$ = new BehaviorSubject<VisualizarEvento>({} as VisualizarEvento);
  visualizarRecolher$ = new BehaviorSubject<VisualizarEvento>({} as VisualizarEvento);
  visualizarConcluido$ = new BehaviorSubject<VisualizarEvento>({} as VisualizarEvento);
  enviando!: boolean;

  constructor(
    private pedidoService: PedidoService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private snackBar: SnackResponseService
  ) { }
  ngOnInit(): void {
    this.route.paramMap
      .pipe(
        switchMap((x: any) => {
          if (!x.params.id) {
            return of<VisualizarPedido>();
          }
          this.visualizarPedido$.value.id = x.params.id;
          return this.pedidoService.ObterPedidoPorId(this.visualizarPedido$.value.id);
        })
      )
      .subscribe((x: VisualizarPedido) => {
        if (x) {
          this.pedidoItem$.next(x.pedidoItem);
          this.visualizarPedido$.next(x);
          this.obterEventoNotaFiscal(this.visualizarPedido$.value.eventos);
          this.obterEventoCTR(this.visualizarPedido$.value.eventos);
          this.obterEventoRecolher(this.visualizarPedido$.value.eventos);
          this.obterEventoConcluido(this.visualizarPedido$.value.eventos);
        }
      });
  }
  emitirNotaFiscal() {
    this.enviando = true;
    let notaFiscal = {} as NotaFiscal;
    notaFiscal.pedidoId = this.visualizarPedido$.value.id;
    this.pedidoService
      .emitirNotaFiscal(this.visualizarPedido$.value.id, notaFiscal)
      .subscribe((x) => {
        this.enviando = false;
        this.visualizarNotaFiscal$.value.status = StatusPedido.aguardando;
        this.snackBar.mostrarMensagem("Processo de emissão de nota fiscal em andamento! Aguarde resposta da prefeitura.");
      }, (err) => {
        this.snackBar.mostrarMensagem("Erro ao emitir nota fiscal!", true);
      });
    this.enviando = true;
  }

  statusPedido() {
    this.enviando = true;
    let statusNotaFiscal = {} as StatusNotaFiscal;
    statusNotaFiscal.pedidoId = this.visualizarPedido$.value.id;
    this.pedidoService
      .consultarStatusNotaFiscal(this.visualizarPedido$.value.id, statusNotaFiscal)
      .subscribe((resposta: any) => {
        this.enviando = false;
        resposta.dados.forEach((x: IStatusPedidoOmie) => {
          if (x.nNfse) {
            this.visualizarNotaFiscal$.value.status = StatusPedido.concluido;
            this.visualizarPedido$.value.numeroNotaFiscal = x.nNfse;
          }
        });
        this.visualizarPedido$.value.numeroNotaFiscal == '' ?
          this.abrirPopupInformativo(resposta)
          : of();
      }, (err) => {
        this.enviando = false;
        this.snackBar.mostrarMensagem(err.error.mensagem, true);
      });
    this.enviando = true;
  }
  escolherCacamba() {
    const dialogRef = this.dialog.open(SelecionarCacambaComponent, {
      width: "30%",
      autoFocus: false,
    });
    dialogRef.afterClosed()
      .subscribe((cacamba: VisualizarCacamba) => {
        if (cacamba) {
          this.vincularCacamba(cacamba);
        }
        return of();
      }, (e) => {
        this.snackBar.mostrarMensagem(e.error.mensagem, true);
      });
  }

  emitirCTR() {
    this.dialog.open(EmitirCtrComponent, {
      data: this.visualizarPedido$.value,
      autoFocus: false
    });
  }
  recolherCacamba() {
    this.dialog.open(EmitirCtrComponent, {
      data: this.visualizarPedido$.value,
      autoFocus: false
    });
  }
  private obterEventoNotaFiscal(eventos: VisualizarEvento[]) {
    let eventoNotaFiscal = eventos.filter((x) => x.descricao == 'NotaFiscal')[0];
    this.visualizarNotaFiscal$.next(eventoNotaFiscal);
  }
  private obterEventoCTR(eventos: VisualizarEvento[]) {
    let eventoCTR = eventos.filter((x) => x.descricao == 'CTR')[0];
    this.visualizarCTR$.next(eventoCTR);
  }
  private obterEventoRecolher(eventos: VisualizarEvento[]) {
    let eventoRecolher = eventos.filter((x) => x.descricao == 'Recolher')[0];
    this.visualizarRecolher$.next(eventoRecolher);
  }
  private obterEventoConcluido(eventos: VisualizarEvento[]) {
    let eventoConcluido = eventos.filter((x) => x.descricao == 'Concluido')[0];
    this.visualizarConcluido$.next(eventoConcluido);
  }

  private abrirPopupInformativo(resposta: any) {
    this.dialog.open(PopupInformativoComponent, {
      data: { info: resposta }
    });
  }
  private vincularCacamba(cacamba: VisualizarCacamba) {
    let vincularCacamba = {} as VincularCacamba;
    vincularCacamba.cacambaId = cacamba.id;
    vincularCacamba.pedidoId = this.visualizarPedido$.value.id;
    this.pedidoService.vincularCacamba(this.visualizarPedido$.value.id, vincularCacamba).subscribe(() => {
      this.pedidoItem$.value.cacamba = cacamba;
      this.snackBar.mostrarMensagem("Caçamba vinculada com sucesso!")
    }, (e) => {
      this.enviando = false;
      this.snackBar.mostrarMensagem(e.error.mensagem, true);
    })
  }
}
