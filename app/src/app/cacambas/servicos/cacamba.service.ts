import { Paginacao } from '../../identidade-acesso/interfaces/paginacao';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import {
  AlterarCacamba,
  NovaCacamba,
  VisualizarCacamba,
} from '../interfaces/icacamba';
import { map } from 'rxjs/operators';
const url = `${environment.urlApi}/cacamba`;

@Injectable({
  providedIn: 'root',
})
export class CacambaServico {
  constructor(private http: HttpClient) { }

  obter(
    pageIndex: number = 0,
    pageSize: number = 10,
    numero: string = '',
    volume: string = ''
  ): Observable<Paginacao<VisualizarCacamba>> {
    let params = new HttpParams()
      .append('pageIndex', pageIndex)
      .append('pageSize', pageSize)
      .append('numero', numero)
      .append('volume', volume);
    return this.http.get<Paginacao<VisualizarCacamba>>(url, {
      params: params,
    });
  }

  obterCacambasDisponiveis(): Observable<any> {
    return this.http.get<any>(`${url}/cacambas-disponiveis`);
  }
  criar(novaCacamba: NovaCacamba) {
    return this.http.post(url, novaCacamba);
  }
  alterar(alterarCacamba: AlterarCacamba): Observable<any> {
    return this.http.put(`${url}/${alterarCacamba.id}`, alterarCacamba);
  }
  excluir(id: number): Observable<any> {
    return this.http.delete(`${url}/${id}`);
  }

  obterApenasCacambas(): Observable<VisualizarCacamba[]> {
    return this.http.get<VisualizarCacamba[]>(url).pipe(
      map((x: any) => x["data"])
    )
  }
}
