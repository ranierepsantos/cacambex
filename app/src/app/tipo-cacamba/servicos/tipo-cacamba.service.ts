import { Paginacao } from '../../identidade-acesso/interfaces/paginacao';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { VisualizarTipoCacamba, AlterarTipoCacamba, ITipoCacambaComPrecoFaixaCep } from '../interfaces/itipo-cacamba';
import { map } from 'rxjs/operators';
const url = `${environment.urlApi}/tipo-cacamba`;

@Injectable({
  providedIn: 'root',
})
export class TipoCacambaServico {
  constructor(private http: HttpClient) { }

  obter(
    pageIndex: number = 0,
    pageSize: number = 10,
    sort: string = 'asc',
  ): Observable<Paginacao<VisualizarTipoCacamba>> {
    let params = new HttpParams()
      .append('pageIndex', pageIndex)
      .append('pageSize', pageSize)
      .append('sort', sort);
    return this.http.get<Paginacao<VisualizarTipoCacamba>>(url, {
      params: params,
    });
  }

  obterPorId(id: number  ): Observable<AlterarTipoCacamba> {
    return this.http.get<AlterarTipoCacamba>(`${url}/${id}`);
  }

  alterar(alterarTipoCacamba: AlterarTipoCacamba): Observable<any> {
    return this.http.put(`${url}/${alterarTipoCacamba.id}`, alterarTipoCacamba);
  }
  
  listarComPrecoFaixaCep(
    cep: string = '',
  ): Observable<ITipoCacambaComPrecoFaixaCep[]> {
    let params = new HttpParams()
      .append('cep', cep);
    return this.http.get<ITipoCacambaComPrecoFaixaCep[]>(`${url}/listar-com-preco-faixa-cep`, {
      params: params,
    });
  }
}
