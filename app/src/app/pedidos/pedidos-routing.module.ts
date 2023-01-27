import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PedidoGuard } from './guard/pedido.guard';

import { EditarPedidoComponent } from './pages/editar-pedido/editar-pedido.component';
import { EnderecosComponent } from './pages/enderecos/enderecos.component';
import { GerenciarPedidoComponent } from './pages/gerenciar-pedido/gerenciar-pedido.component';
import { NovoPedidoComponent } from './pages/novo-pedido/novo-pedido.component';
import { TelaPedidosComponent } from './pages/tela-pedidos/tela-pedidos.component';

const routes: Routes = [
  { path: '', component: TelaPedidosComponent },
  { path: 'novo-pedido', component: NovoPedidoComponent },
  { path: 'novo-pedido/:id', component: NovoPedidoComponent },
  { path: 'nota-fiscal/:id', component: NovoPedidoComponent },
  { path: 'entregue/:id', component: NovoPedidoComponent },
  { path: 'pedidos/enderecos', component: EnderecosComponent },
  { path: 'gerenciar-pedido/:id', component: GerenciarPedidoComponent },
  { path: 'editar-pedido/:id', component: EditarPedidoComponent, resolve: { pedido: PedidoGuard } },
  { path: '', pathMatch: 'full', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PedidosRoutingModule { }
