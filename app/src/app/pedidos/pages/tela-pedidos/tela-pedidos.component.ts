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
import { PopupConfirmacaoComponent } from 'src/app/design-system/popup-confirmacao/popup-confirmacao.component';
import { SnackResponseService } from 'src/app/design-system/snack-response.service';
import { Paginacao } from 'src/app/identidade-acesso/interfaces/paginacao';
import { UsuarioDecodificado } from 'src/app/identidade-acesso/interfaces/usuario-decodificado';
import { TokenServico } from 'src/app/identidade-acesso/servicos/token.servico';

import { VisualizarPedido } from '../../interfaces/ipedido';
import { PedidoService } from './../../servicos/pedido.service';
import { ReciboComponent } from '../recibo/recibo.component';
import { relatorioComponent } from '../relatorio/relatorio.component';

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
      : this.obterPedidosComFiltros();
  }

  novoPedido() {
    this.router.navigate(["/pedidos/novo-pedido"]);
  }
  // private obterPedidos(
  //   pageIndex: number = 0,
  //   pageSize: number = 10
  // ) {
  //   this.servicoPedido
  //     .obter(
  //       pageIndex,
  //       pageSize,
  //       this.sort
  //     )
  //     .subscribe((x) => {
  //       this.dataSource = x;
  //       this.load = true;
  //     }, (e) => {
  //       this.snackBar.mostrarMensagem("Erro ao carregar pedidos. Tente contato com o suporte.", true);
  //     });
  // }
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
      },
        (e) => {
          this.snackBar.mostrarMensagem("Erro ao carregar pedidos. Tente recarregar a página. Em caso de dúvidas contatar o suporte.", true);
          this.snackBar.mostrarMensagem(`${e.message}`, true);
        });
  }
  private obterPedidosComFiltros(
    pageIndex = 0,
    pageSize = 10,
    documentoCliente: string = "",
    nomeCliente: string = "",
    notaFiscal: string ="",
    numeroCTR: string ="",
    dataInicio: Date = new Date(),
    dataFim: Date = new Date(),
    filtrarData: boolean = false,
    sort: string ="desc"
  ) {
    this.servicoPedido.obterPedidoComFiltro(
      pageIndex,
      pageSize,
      documentoCliente,
      nomeCliente,
      notaFiscal,
      numeroCTR,
      dataInicio,
      dataFim,
      filtrarData,
      sort
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
  emitirRecibo(pedido: VisualizarPedido) {
    this.dialog.open(ReciboComponent, {
      width: '750px',
      data: pedido,
    });
  }
  imprimirRelatorio() {
    let result: any;
    this.servicoPedido.obterPedidoComFiltro(0, this.dataSource.totalCount,
      this.filtroDocumentoCliente,
      this.filtroNomeCliente,
      this.filtroNumeroNotaFiscal,
      this.filtroNumeroCTR,
      this.filtroDataInicio,
      this.filtroDataFim,
      this.filtrarPorData,
      this.sort).subscribe((x) => {
        this.servicoPedido.setPedidos(x.data)
        this.router.navigate(["pedidos/relatorio-pedido"]);
      }, (e) => {
        this.snackBar.mostrarMensagem("Erro ao carregar pedidos. Tente novamente em 1 minuto.", true);
      })
  }

  mudarPagina(e: PageEvent) {
    this.usuarioDecodificado.role == 'Cliente' ?
      this.obterPedidosPorClienteId(this.usuarioDecodificado.nameid, e.pageIndex, e.pageSize)
      : this.aplicandoFiltros(e.pageIndex, e.pageSize);
  }
  aplicandoFiltros(page: number = 0, pageSize: number = 10) {
    this.filtroNomeCliente = this.filtroNomeCliente;
    this.filtroNumeroCTR = this.filtroNumeroCTR;
    this.filtroNumeroNotaFiscal = this.filtroNumeroNotaFiscal;
    this.filtroDocumentoCliente = this.filtroDocumentoCliente;
    this.filtroDataInicio = this.filtroDataInicio;
    this.filtroDataFim = this.filtroDataFim;
    this.filtrarPorData = this.filtrarPorData;
    this.obterPedidosComFiltros(page, pageSize, this.filtroDocumentoCliente, this.filtroNomeCliente, this.filtroNumeroNotaFiscal, this.filtroNumeroCTR, this.filtroDataInicio, this.filtroDataFim, this.filtrarPorData,  this.sort);
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
