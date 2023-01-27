import { TokenServico } from './../servicos/token.servico';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
@Injectable({
  providedIn: 'root',
})
export class AutorizacaoGuarda implements CanActivate {
  constructor(private tokenServico: TokenServico, private router: Router) {}
  canActivate() {
    const activate = this.tokenServico.token != null;
    if (!activate) this.router.navigateByUrl('/identidade-acesso');
    return activate;
  }
}
