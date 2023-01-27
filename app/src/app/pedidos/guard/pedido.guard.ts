import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';

import { StatusPedido } from '../enums/status-pedido';
import { TipoDePagamento } from '../enums/tipo-pagamento';
import { PedidoService } from '../servicos/pedido.service';
import { VisualizarPedido } from './../interfaces/ipedido';

@Injectable({
  providedIn: 'root'
})
export class PedidoGuard implements Resolve<VisualizarPedido> {
  constructor(private pedidoService: PedidoService) { }
  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<VisualizarPedido> {
    if (route.params && route.params['id']) {
      return this.pedidoService.ObterPedidoPorId(route.params['id'])
    } else {

      return of(
        //   {
        //   id: 0,
        //   nomeCliente: '',
        //   idCliente: 0,
        //   documentoCliente: '',
        //   telefoneCliente: '',
        //   emailCliente: '',
        //   numeroNotaFiscal: '',
        //   tipoDePagamento: TipoDePagamento.boleto,
        //   pedidoItem: {
        //     id: 0,
        //     volumeCacamba: '',
        //     cacamba: {
        //       id: 0,
        //       numero: '',
        //       volume: '',
        //       preco: 0,
        //     },
        //     valorUnitario: 0,
        //     ctr: {
        //       id: 0,
        //       quando: new Date,
        //       descricao: '',
        //       status: StatusPedido.concluido,
        //       mensagem: '',
        //     },
        //     mtr: {
        //       id: 0,
        //       quando: new Date,
        //       descricao: '',
        //       status: StatusPedido.concluido,
        //       mensagem: '',
        //     },
        //   },
        //   enderecoEntrega: {
        //     id: 0,
        //     cep: '',
        //     logradouro: '',
        //     numero: '',
        //     complemento: '',
        //     bairro: '',
        //     cidade: '',
        //     uf: '',
        //   },
        //   valorPedido: 0,
        //   observacao: '',
        //   eventos: [],
        //   emitidoEm: new Date,
        // }
      );
    }

  }

}
