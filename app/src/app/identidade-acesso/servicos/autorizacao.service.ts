import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Resposta } from 'src/app/design-system/interfaces/resposta';
import { environment } from 'src/environments/environment';

import { AlterarSenha, AutorizacaoUsuario, RecuperarSenha } from '../interfaces/autorizacao-usuario';
import { TokenServico } from './token.servico';

const url = `${environment.urlApi}/autenticacao`;

@Injectable({
  providedIn: 'root',
})
export class AutorizacaoServico {
  constructor(
    private http: HttpClient,
    private tokenServico: TokenServico,
    private router: Router
  ) { }
  login(autorizacaoUsuario: AutorizacaoUsuario): Observable<any> {
    return this.http.post(url, autorizacaoUsuario);
  }

  recuperarSenha(recuperarSenha: RecuperarSenha): Observable<Resposta> {
    return this.http.post<Resposta>(`${url}/recuperar-senha`, recuperarSenha);
  }

  alterarSenha(alterarSenha: AlterarSenha, token: string): Observable<any> {
    return this.http.put(url, alterarSenha, {
      headers: { Authorization: `Bearer ${token}` },
    });
  }

  logout() {
    this.tokenServico.removeToken();
    this.router.navigateByUrl('identidade-acesso');
  }
}
