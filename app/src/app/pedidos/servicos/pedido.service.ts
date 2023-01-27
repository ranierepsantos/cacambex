import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Paginacao } from 'src/app/identidade-acesso/interfaces/paginacao';
import { environment } from 'src/environments/environment';

import { NotaFiscal, StatusNotaFiscal } from '../interfaces/inota-fiscal';
import { VisualizarPedido } from '../interfaces/ipedido';
import { VincularCacamba } from '../interfaces/vincular-cacamba';
import { EnviarCacambaObra, RecolherCacambaObra, SolicitarCTR } from './../interfaces/ictr';

const url = `${environment.urlApi}/pedido`;

@Injectable({
  providedIn: 'root',
})
export class PedidoService {
  constructor(private http: HttpClient) { }
  vincularCacamba(
    pedidoId: number,
    vincularCacamba: VincularCacamba
  ): Observable<any> {
    return this.http.put(
      `${url}/vincular-cacamba/${pedidoId}`,
      vincularCacamba
    )
  }

  enviarCacamba(
    pedidoId: number,
    cacamba: EnviarCacambaObra
  ): Observable<any> {
    return this.http.put(
      `${url}/enviar-cacamba/${pedidoId}`,
      cacamba
    );
  }

  recolherCacamba(
    pedidoId: number,
    cacamba: RecolherCacambaObra
  ): Observable<any> {
    return this.http.put(
      `${url}/retirar-cacamba/${pedidoId}`,
      cacamba
    );
  }
  solicitarCTR(
    pedidoId: number,
    novoCTR: SolicitarCTR
  ): Observable<any> {
    return this.http.put(
      `${url}/emitir-ctr/${pedidoId}`,
      novoCTR
    );
  }
  emitirNotaFiscal(
    pedidoId: number,
    notaFiscal: NotaFiscal
  ): Observable<any> {
    return this.http.put(
      `${url}/emitir-nota-fiscal/${pedidoId}`,
      notaFiscal
    );
  }

  consultarStatusNotaFiscal(
    pedidoId: number,
    statusNotaFiscal: StatusNotaFiscal
  ): Observable<any> {
    return this.http.put(
      `${url}/consultar-status-nota-fiscal/${pedidoId}`,
      statusNotaFiscal
    );
  }
  ObterPedidoPorId(pedidoId: number) {
    return this.http.get<VisualizarPedido>(`${url}/${pedidoId}`);
  }
  obterPedidosPorClienteId(
    clienteId: number,
    pageIndex: number = 0,
    pageSize: number = 10,
    sort: string = 'desc'): Observable<Paginacao<VisualizarPedido>> {
    let params = new HttpParams()
      .append('pageIndex', pageIndex)
      .append('pageSize', pageSize)
      .append('sort', sort);
    return this.http.get<Paginacao<VisualizarPedido>>
      (`${url}/obter-pedidos-cliente/${clienteId}`, {
        params: params,
      });
  }

  obter(
    pageIndex: number = 0,
    pageSize: number = 10,
    sort: string = 'desc'
  ): Observable<Paginacao<VisualizarPedido>> {
    let params = new HttpParams()
      .append('pageIndex', pageIndex)
      .append('pageSize', pageSize)
      .append('sort', sort);
    return this.http.get<Paginacao<VisualizarPedido>>(url, {
      params: params,
    });
  }


  obterPedidoComFiltro(
    pageIndex: number = 0,
    pageSize: number = 10,
    documentoCliente: string = "",
    nomeCliente: string = "",
    sort: string = "desc"
  ): Observable<Paginacao<VisualizarPedido>> {

    let params_ = new HttpParams()
      .append('pageIndex', pageIndex)
      .append('pageSize', pageSize)
      .append('documentoCliente', documentoCliente)
      .append('nomeCliente', nomeCliente)
      .append('sort', sort);

    let params = this.removeParametrosVaziosOuNulos(params_);
    return this.http.get<Paginacao<VisualizarPedido>>(url, {
      params: params,
    });
  }
  criarPedido(novoPedido: any) {
    return this.http.post(url, novoPedido);
  }
  alterarPedido(alterarPedido: any): Observable<any> {
    return this.http.put(`${url}/${alterarPedido.pedidoId}`, alterarPedido);
  }
  excluir(id: number): Observable<any> {
    return this.http.delete(`${url}/${id}`);
  }

  private removeParametrosVaziosOuNulos(params: HttpParams) {
    const paramsKeysAux = params.keys();
    paramsKeysAux.forEach((key) => {
      const value = params.get(key);
      if (value === null || value === undefined || value === '') {
        params['map'].delete(key);
      }
    });

    return params;
  }
}
