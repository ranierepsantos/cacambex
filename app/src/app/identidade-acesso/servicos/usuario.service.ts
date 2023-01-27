import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AlterarUsuario, NovoUsuario, VisualizarUsuario } from '../interfaces/iusuario';
import { Paginacao } from '../interfaces/paginacao';

const url = `${environment.urlApi}/usuario`;

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {

  constructor(private http: HttpClient) { }
  
  obter(
    pageIndex: number = 0,
    pageSize: number = 10,
    nome: string = "",
    email: string = "",
    ): Observable<Paginacao<VisualizarUsuario>> {
      let params = new HttpParams()
      .append("pageIndex", pageIndex)
      .append("pageSize", pageSize)
      .append("nome", nome)
      .append("email", email)
    return this.http.get<Paginacao<VisualizarUsuario>>(url, {
      params: params
    });
  }
  criar(novoUsuario: NovoUsuario) {
    return this.http.post(url, novoUsuario);
  }
  alterar(alterarUsuario: AlterarUsuario): Observable<any> {
    return this.http.put(`${url}/${alterarUsuario.id}`, alterarUsuario);
  }
  excluir(id: number): Observable<any> {
    return this.http.delete(`${url}/${id}`);
  }
}
