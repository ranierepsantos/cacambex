import { SnackResponseService } from './../../../design-system/snack-response.service';
import { Router } from '@angular/router';
import { ClienteService } from '../../servicos/cliente.service';
import { VisualizarCliente } from '../../interfaces/icliente';
import { Component, OnInit } from '@angular/core';
import { Paginacao } from 'src/app/identidade-acesso/interfaces/paginacao';
import { PageEvent } from '@angular/material/paginator';

@Component({
  templateUrl: './clientes.component.html',
  styleUrls: ['./clientes.component.css'],
})
export class ClientesComponent implements OnInit {
  displayedColumns = ['nome', 'telefone', 'documento', 'email', 'acao'];
  dataSource: Paginacao<VisualizarCliente> = {} as Paginacao<VisualizarCliente>;
  constructor(
    private snackBar: SnackResponseService,
    private router: Router,
    private clienteService: ClienteService,
  ) {
    this.dataSource.data = [];
  }

  ngOnInit(): void {
    this.obterClientes();
  }

  private obterClientes(
    pageIndex: number = 0,
    pageSize: number = 10
  ) {
    this.clienteService
      .obterCliente(pageIndex, pageSize)
      .subscribe((x) => {
        this.dataSource = x;
      }, e => {
        this.snackBar.mostrarMensagem("Ocorreu em erro inesperado. Entre em contato com o suporte.", true)
      });
  }
  excluirCliente(visualizarCliente: VisualizarCliente) {
    this.clienteService.excluirCliente(visualizarCliente.id).subscribe((x) => {
      this.obterClientesComFiltros();
      this.snackBar.mostrarMensagem("Cliente excluído com sucesso.");
    }, (e) => {
      this.snackBar.mostrarMensagem('Não é possivel excluir o cliente. Entre em contato com o suporte para detalhes.', true);
    })
  }
  mudarPagina(e: PageEvent) {
    this.obterClientes(e.pageIndex, e.pageSize);
  }
  private obterClientesComFiltros() {
    this.obterClientes(0, 10);
  }
  novoCliente() {
    this.router.navigate(["novo-cliente"]);
  }
  editarCliente(id: number) {
    this.router.navigate(['editar-cliente', id]);
  }
}
