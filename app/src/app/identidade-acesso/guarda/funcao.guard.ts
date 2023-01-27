import { UsuarioDecodificado } from 'src/app/identidade-acesso/interfaces/usuario-decodificado';
import { TokenServico } from 'src/app/identidade-acesso/servicos/token.servico';
import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class FuncaoGuard implements CanActivate {
  usuario = {} as UsuarioDecodificado;
  constructor(private tokenServico: TokenServico) {}
  canActivate() {
    this.tokenServico.usuario.subscribe((x) => {
      this.usuario = x;
    });
    if (this.usuario.role == 'Administrador') {
      return true;
    }
    return false;
  }
}
