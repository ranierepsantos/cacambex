import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

import { AutoCadastro } from '../interfaces/auto-cadastro';

const url = `${environment.urlApi}/auto-cadastro`;

@Injectable({
  providedIn: 'root',
})
export class AutoCadastroServico {
  constructor(private http: HttpClient) { }

  criar(autoCadastro: AutoCadastro) {
    return this.http.post(url, autoCadastro);
  }
}
