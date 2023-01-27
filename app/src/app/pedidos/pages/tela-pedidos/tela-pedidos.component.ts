import { Component, OnInit } from '@angular/core';
import {
  MAT_MOMENT_DATE_ADAPTER_OPTIONS,
  MAT_MOMENT_DATE_FORMATS,
  MomentDateAdapter,
} from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE, ThemePalette } from '@angular/material/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { EMPTY } from 'rxjs';
import { PopupConfirmacaoComponent } from 'src/app/design-system/popup-confirmacao/popup-confirmacao.component';
import { SnackResponseService } from 'src/app/design-system/snack-response.service';
import { Paginacao } from 'src/app/identidade-acesso/interfaces/paginacao';
import { UsuarioDecodificado } from 'src/app/identidade-acesso/interfaces/usuario-decodificado';
import { TokenServico } from 'src/app/identidade-acesso/servicos/token.servico';

import { VisualizarPedido } from '../../interfaces/ipedido';
import { PedidoService } from './../../servicos/pedido.service';

@Component({
  templateUrl: "./tela-pedidos.component.html",
  styleUrls: ["./tela-pedidos.component.css"],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: "pt-BR" },
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS],
    },
    { provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS },
  ],
})
export class TelaPedidosComponent implements OnInit {
  tituloCabecalho: string = "Pedidos";
  titulo: string = "Filtros";
  filtroNomeCliente: string = "";
  filtroDocumentoCliente: string = "";
  filtroNumeroNotaFiscal: string = "";
  filtroNumeroCTR: string = "";
  filtroDataInicio: Date = new Date();
  filtroDataFim: Date = new Date();
  filtrarPorData: boolean = false;
  tipoUsuario!: UsuarioDecodificado;
  sort: string = "desc";
  load: boolean = true;
  color: ThemePalette = "primary";
  usuarioDecodificado: UsuarioDecodificado = {} as UsuarioDecodificado;

  dataSource: Paginacao<VisualizarPedido> = {} as Paginacao<VisualizarPedido>;
  constructor(
    private tokenServico: TokenServico,
    private servicoPedido: PedidoService,
    private router: Router,
    private dialog: MatDialog,
    private snackBar: SnackResponseService
  ) {
    this.tokenServico.usuario.subscribe((x => {
      this.usuarioDecodificado = x;
    }));
  }

  ngOnInit(): void {
    this.obterPedidosGeral();
  }

  private obterPedidosGeral() {
    this.usuarioDecodificado.role == 'Cliente' ?
      this.obterPedidosPorClienteId(this.usuarioDecodificado.nameid)
      : this.obterPedidos();
  }

  novoPedido() {
    this.router.navigate(["/pedidos/novo-pedido"]);
  }
  private obterPedidos(
    pageIndex: number = 0,
    pageSize: number = 10
  ) {
    this.servicoPedido
      .obter(
        pageIndex,
        pageSize,
        this.sort
      )
      .subscribe((x) => {
        this.dataSource = x;
        this.load = true;
      }, (e) => {
        this.snackBar.mostrarMensagem("Erro ao carregar pedidos. Tente novamente em 1 minuto.", true);
      });
  }
  private obterPedidosPorClienteId(
    clienteId: any,
    pageIndex: number = 0,
    pageSize: number = 10) {
    const id = parseInt(clienteId);
    this.servicoPedido.obterPedidosPorClienteId(
      id,
      pageIndex,
      pageSize,
      this.sort).subscribe((x) => {
        this.dataSource = x;
        this.load = true;
      });
  }
  private obterPedidosComFiltros(
    pageIndex = 0,
    pageSize = 10,
    documentoCliente: string = "",
    nomeCliente: string = "",
  ) {
    this.servicoPedido.obterPedidoComFiltro(
      pageIndex,
      pageSize,
      documentoCliente,
      nomeCliente,
    ).subscribe((x) => {
      this.dataSource = x;
      this.load = true;
    }, (e) => {
      this.snackBar.mostrarMensagem("Erro ao carregar pedidos. Tente novamente em 1 minuto.", true);
    })
  }
  editarPedido(visualizarPedido: VisualizarPedido) {
    this.router.navigate(["pedidos/editar-pedido", visualizarPedido.id]);
  }
  gerenciarPedido(visualizarPedido: VisualizarPedido) {
    this.router.navigate(["pedidos/gerenciar-pedido", visualizarPedido.id])
  }
  mudarPagina(e: PageEvent) {
    this.usuarioDecodificado.role == 'Cliente' ?
      this.obterPedidosPorClienteId(this.usuarioDecodificado.nameid, e.pageIndex, e.pageSize)
      : this.obterPedidos(e.pageIndex, e.pageSize);
  }
  aplicandoFiltros() {
    this.filtroNomeCliente = this.filtroNomeCliente;
    this.filtroNumeroCTR = this.filtroNumeroCTR;
    this.filtroNumeroNotaFiscal = this.filtroNumeroNotaFiscal;
    this.filtroDocumentoCliente = this.filtroDocumentoCliente;
    this.filtroDataInicio = this.filtroDataInicio;
    this.filtroDataFim = this.filtroDataFim;
    this.filtrarPorData = this.filtrarPorData;
    this.obterPedidosComFiltros(0, 10, this.filtroDocumentoCliente, this.filtroNomeCliente);
  }

  excluirPedido(visualizarPedido: VisualizarPedido) {
    const dialogRef = this.dialog.open(PopupConfirmacaoComponent, {
      width: '250px',
      data: { titulo: 'o pedido', identificador: visualizarPedido.id }
    });
    dialogRef.afterClosed().subscribe((resposta) => {
      if (resposta == true) {
        this.servicoPedido.excluir(visualizarPedido.id).subscribe(() => {
          this.obterPedidosGeral();
        });
      }
    });
  }
}
