import { Injectable } from '@angular/core';
import jwt_decode from 'jwt-decode';
import { BehaviorSubject } from 'rxjs';
import { TOKEN_STORAGE } from 'src/environments/environment';

import { UsuarioDecodificado } from '../interfaces/usuario-decodificado';

@Injectable({
  providedIn: 'root',
})
export class TokenServico {
  private _usuario = new BehaviorSubject<UsuarioDecodificado>(
    this.decodePayloadJWT()
  );
  usuario = this._usuario.asObservable();
  constructor() { }

  get token(): any {
    return localStorage.getItem(TOKEN_STORAGE);
  }

  set token(token: any) {
    localStorage.setItem(TOKEN_STORAGE, token as string);
    this._usuario.next(this.decodePayloadJWT());
  }

  removeToken() {
    localStorage.removeItem(TOKEN_STORAGE);
  }

  private decodePayloadJWT(): any {
    try {
      return jwt_decode(this.token as string);
    } catch (error) {
      return null;
    }
  }
}
