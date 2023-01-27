import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { SnackResponseService } from 'src/app/design-system/snack-response.service';
import { Paginacao } from 'src/app/identidade-acesso/interfaces/paginacao';
import { environment } from 'src/environments/environment';

import { AlterarCliente, NovoCliente, VisualizarCliente } from '../interfaces/icliente';
import { NovoEnderecoComClienteId } from '../interfaces/ienderecos';

const url = `${environment.urlApi}/cliente`;

@Injectable({
  providedIn: 'root'
})
export class ClienteService {

  constructor(private http: HttpClient, private snackBar: SnackResponseService) { }

  ObterClientePorId(id: number) {
    return this.http.get<VisualizarCliente>(`${url}/${id}`);
  }
  obterCliente(
    pageIndex: number = 0,
    pageSize: number = 10
  ): Observable<Paginacao<VisualizarCliente>> {
    let params = new HttpParams()
      .append("pageIndex", pageIndex)
      .append("pageSize", pageSize)
    return this.http.get<Paginacao<VisualizarCliente>>(url, {
      params: params
    });

  }
  criarCliente(novoCliente: NovoCliente) {
    return this.http.post(url, novoCliente).pipe(
      map(x => x),
      catchError((e) => this.snackBar.errorHandler(e))
    );
  }
  alterarCliente(alterarCliente: AlterarCliente): Observable<any> {
    return this.http.put(`${url}/${alterarCliente.id}`, alterarCliente)
      .pipe(
        map(x => x),
        catchError((e) => this.snackBar.errorHandler(e))
      );
  }
  excluirCliente(id: number): Observable<any> {
    return this.http.delete(`${url}/${id}`).pipe(
      map(x => x),
      catchError((e) => this.snackBar.errorHandler(e))
    );
  }
  novoEndereco(endereco: NovoEnderecoComClienteId) {
    return this.http.post<NovoEnderecoComClienteId>(`${url}/novo-endereco-entrega`, endereco);
  }
}
