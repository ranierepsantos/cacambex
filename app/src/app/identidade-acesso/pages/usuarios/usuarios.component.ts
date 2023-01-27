import { SnackResponseService } from './../../../design-system/snack-response.service';
import { EditarUsuarioComponent } from './../editar-usuario/editar-usuario.component';
import { AlterarUsuario, NovoUsuario, VisualizarUsuario } from './../../interfaces/iusuario';
import { Component, OnInit } from '@angular/core';
import { UsuarioService } from '../../servicos/usuario.service';
import { Paginacao } from '../../interfaces/paginacao';
import { PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { switchMap } from "rxjs/operators";
import { of } from 'rxjs';
import { PopupConfirmacaoComponent } from 'src/app/design-system/popup-confirmacao/popup-confirmacao.component';

@Component({
  templateUrl: './usuarios.component.html',
  styleUrls: ['./usuarios.component.css'],
})
export class UsuariosComponent implements OnInit {
  displayedColumns = ['nome', 'email', 'ativo', 'funcao', 'acao'];
  dataSource: Paginacao<VisualizarUsuario> = {} as Paginacao<VisualizarUsuario>;

  constructor(
    private snackBar: SnackResponseService,
    private serviceUsuario: UsuarioService,
    private dialog: MatDialog
  ) {
    this.dataSource.data = [];
  }

  ngOnInit(): void {
    this.obterUsuarios();
  }
  private obterUsuarios(
    pageIndex: number = 0,
    pageSize: number = 10,
    nome: string = '',
    email: string = ''
  ) {
    this.serviceUsuario
      .obter(pageIndex, pageSize, nome, email)
      .subscribe((x) => {
        this.dataSource = x;
      });
  }
  novoUsuario() {
    const dialogRef = this.dialog.open(EditarUsuarioComponent, {
      width: '400px'
    });
    dialogRef.afterClosed().pipe(switchMap((x: NovoUsuario) => {
      if(x == undefined) return of();
      return this.serviceUsuario.criar(x);
    })).subscribe((x) => {
      this.obterUsuarios();
      this.snackBar.mostrarMensagem("Usuário criado com sucesso.");
    }, (erro: any) => {
      this.snackBar.mostrarMensagem(erro.error, true)
    })
  }
  alterarUsuario(usuario: VisualizarUsuario) {
    const dialogRef = this.dialog.open(EditarUsuarioComponent, {
      width: '400px',
      data: usuario
    });
    dialogRef.afterClosed().pipe(switchMap((x: AlterarUsuario) => {
      if(x == undefined) return of();
      return this.serviceUsuario.alterar(x);
    })).subscribe((x) => {
      this.obterUsuarios();
      this.snackBar.mostrarMensagem("Usuário alterado com sucesso.");
    })
  }
  excluirUsuario(visualizarUsuario: VisualizarUsuario) {
    const dialogRef = this.dialog.open(PopupConfirmacaoComponent, {
      width: '400px',
    })
    dialogRef.afterClosed().subscribe((x) => {
      if (x) {
        this.serviceUsuario.excluir(visualizarUsuario.id).subscribe(() => {
          this.obterUsuariosComFiltros();
          this.snackBar.mostrarMensagem("Usuário excluído com sucesso.");
        });
      }
    })
  }
  mudarPagina(e: PageEvent) {
    this.obterUsuarios(e.pageIndex, e.pageSize);
  }
  mudarFiltro(nome: string, email: string) {
    this.obterUsuarios(0, 10, nome, email);
  }
  private obterUsuariosComFiltros() {
    this.obterUsuarios(0, 10, '', '');
  }
}
