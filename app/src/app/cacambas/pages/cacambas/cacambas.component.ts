import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { of } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { PopupConfirmacaoComponent } from 'src/app/design-system/popup-confirmacao/popup-confirmacao.component';
import { SnackResponseService } from 'src/app/design-system/snack-response.service';

import { Paginacao } from '../../../identidade-acesso/interfaces/paginacao';
import { AlterarCacamba, NovaCacamba, VisualizarCacamba } from '../../interfaces/icacamba';
import { CacambaServico } from '../../servicos/cacamba.service';
import { EditarCacambaComponent } from '../editar-cacamba/editar-cacamba.component';

@Component({
  templateUrl: './cacambas.component.html',
  styleUrls: ['./cacambas.component.css'],
})
export class CacambasComponent implements OnInit {
  displayedColumns = ['numero', 'volume', 'preco', 'status', 'acao'];
  dataSource: Paginacao<VisualizarCacamba> = {} as Paginacao<VisualizarCacamba>;
  carregando = true;
  constructor(
    private snackBar: SnackResponseService,
    private servicoCacamba: CacambaServico,
    private dialog: MatDialog
  ) {
    this.dataSource.data = [];
  }

  ngOnInit(): void {
    this.obterCacambas();
  }
  private obterCacambas(
    pageIndex: number = 0,
    pageSize: number = 10,
    numero: string = '',
    volume: string = ''
  ) {
    this.carregando = true;
    this.servicoCacamba
      .obter(pageIndex, pageSize, numero, volume)
      .subscribe((x) => {
        this.dataSource = x;
        this.carregando = false;
      }, (e) => {
        this.carregando = true;

      });
  }
  novaCacamba() {
    const dialogRef = this.dialog.open(EditarCacambaComponent, {
      width: '400px',
    });
    dialogRef
      .afterClosed()
      .pipe(
        switchMap((x: NovaCacamba) => {
          if (x == undefined) return of();
          this.snackBar.mostrarMensagem("Processando...");
          return this.servicoCacamba.criar(x);
        })
      )
      .subscribe((x) => {
        this.obterCacambas();
        this.snackBar.mostrarMensagem("Caçamba criada com sucesso.");
      }, (e: any) => {
        console.log(e);

        if (e.error.mensagem) {
          this.snackBar.mostrarMensagem(e.error.mensagem, true)
        }
      });
  }
  alterarCacamba(cacamba: VisualizarCacamba) {
    const dialogRef = this.dialog.open(EditarCacambaComponent, {
      width: '400px',
      data: cacamba,
    });
    dialogRef
      .afterClosed()
      .pipe(
        switchMap((x: AlterarCacamba) => {
          if (x == undefined) return of();
          this.snackBar.mostrarMensagem("Processando...");
          return this.servicoCacamba.alterar(x);
        })
      )
      .subscribe((x) => {
        this.obterCacambas();
        this.snackBar.mostrarMensagem("Caçamba alterada com sucesso.");
      }, (e: any) => {
        this.snackBar.mostrarMensagem(e.error.mensagem, true)
      });
  }
  excluirCacamba(visualizarCacamba: VisualizarCacamba) {
    const dialogRef = this.dialog.open(PopupConfirmacaoComponent, {
      width: '250px',
      data: { titulo: 'a caçamba', identificador: visualizarCacamba.numero }
    })
    dialogRef.afterClosed().subscribe((x) => {
      if (x) {
        this.servicoCacamba.excluir(visualizarCacamba.id).subscribe(() => {
          this.obterCacambasComFiltros();
          this.snackBar.mostrarMensagem("Caçamba excluída com sucesso.");
        });
      }
    })
  }
  mudarPagina(e: PageEvent) {
    this.obterCacambas(e.pageIndex, e.pageSize);
  }
  mudarFiltro(numero: string, volume: string) {
    this.obterCacambas(0, 10, numero, volume);
  }
  private obterCacambasComFiltros() {
    this.obterCacambas(0, 10, '', '');
  }
}
