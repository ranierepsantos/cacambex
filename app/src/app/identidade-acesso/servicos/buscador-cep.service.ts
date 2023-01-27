import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ICEP } from '../interfaces/cep';
const url = `${environment.urlApi}/buscar-endereco`;

@Injectable({
  providedIn: 'root'
})
export class BuscadorCepService {

  constructor(private http: HttpClient) { }

  public getAddress(zipCode: string = ""): Observable<ICEP> {
    let params = new HttpParams().append("cep", zipCode)
    return this.http.get<ICEP>(url, { params: params });
  }
}
