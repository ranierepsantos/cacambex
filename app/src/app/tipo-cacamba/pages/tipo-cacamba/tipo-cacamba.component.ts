import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { of } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { PopupConfirmacaoComponent } from 'src/app/design-system/popup-confirmacao/popup-confirmacao.component';
import { SnackResponseService } from 'src/app/design-system/snack-response.service';

import { Paginacao } from '../../../identidade-acesso/interfaces/paginacao';
import { TipoCacambaServico } from '../../servicos/tipo-cacamba.service';
import { AlterarTipoCacamba, VisualizarTipoCacamba } from '../../interfaces/itipo-cacamba';
import { EditarTipoComponent } from '../editar-tipo/editar-tipo/editar-tipo.component';

@Component({
  selector: 'ca-tipo-cacamba',
  templateUrl: './tipo-cacamba.component.html',
  styleUrls: ['./tipo-cacamba.component.css']
})
export class TipoCacambaComponent implements OnInit {
  displayedColumns = ['volume', 'preco', 'acao'];
  dataSource: Paginacao<VisualizarTipoCacamba> = {} as Paginacao<VisualizarTipoCacamba>;
  carregando = true;
  
  constructor( 
    private snackBar: SnackResponseService,
    private apiService: TipoCacambaServico,
    private dialog: MatDialog
  ) {
    this.dataSource.data = [];
  }

  ngOnInit(): void {
    this.obterDados();
  }

  private obterDados (
    page: number = 0,
    pageSize: number =10,
    sort: string = "asc"
  ) {
    this.carregando = true;
    this.apiService
        .obter(page, pageSize, sort)
        .subscribe((tipos) => {
          this.dataSource = tipos;
          this.carregando = false;
        }, (ex) => {
          this.carregando = true;
        })
  }
  mudarPagina(e: PageEvent) {
    this.obterDados(e.pageIndex, e.pageSize);
  }

  alterarTipoCacamba(tipoCacamba: VisualizarTipoCacamba) {
    const dialogRef = this.dialog.open(EditarTipoComponent, {
      width: '750px',
      data: tipoCacamba,
    });
    dialogRef
      .afterClosed()
      .pipe(
        switchMap((data: any) => {
          if (data == undefined) return of();
          this.snackBar.mostrarMensagem("Processando...");
          
          return this.apiService.alterar(data);
        })
      )
      .subscribe((x) => {
        this.obterDados();
        this.snackBar.mostrarMensagem("Tipo Caçamba alterada com sucesso.");
      }, (e: any) => {
        this.snackBar.mostrarMensagem(e.error.mensagem, true)
      });
  }
}
