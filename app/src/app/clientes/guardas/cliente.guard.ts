import { TipoDocumento } from 'src/app/clientes/enum/tipoDocumento';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { NovoCliente } from '../interfaces/icliente';
import { ClienteService } from '../servicos/cliente.service';

@Injectable({
  providedIn: 'root'
})
export class ClienteGuard implements Resolve<NovoCliente> {
  constructor(private clienteService: ClienteService) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<NovoCliente> {
    if (route.params && route.params['id']) {
      return this.clienteService.ObterClientePorId(route.params['id'])
    } else {
      return of({
        nome: '',
        documento: '',
        tipoDocumento: TipoDocumento.Cnpj,
        dataNascimento: new Date,
        telefone: '',
        email: '',
        contribuinte: '',
        enderecoCobranca: {
          cep: '',
          logradouro: '',
          numero: '',
          complemento: '',
          bairro: '',
          cidade: '',
          uf: '',
          nomeContato: '',
          observacao: '',
        },
        enderecosEntrega: []
      });
    }
  }


}
