import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AutorizacaoGuarda } from './identidade-acesso/guarda/autorizacao.guarda';
import { FuncaoGuard } from './identidade-acesso/guarda/funcao.guard';

const routes: Routes = [
  {
    path: 'identidade-acesso',
    loadChildren: () =>
      import('./identidade-acesso/identidade-acesso.module').then(
        (m) => m.IdentidadeAcessoModule
      ),
  },
  {
    path: 'pedidos',
    loadChildren: () =>
      import('./pedidos/pedidos-routing.module').then(
        (m) => m.PedidosRoutingModule
      ),
    canActivate: [AutorizacaoGuarda],
  },
  {
    path: 'cacambas',
    loadChildren: () =>
      import('./cacambas/cacambas.module').then((m) => m.CacambasModule),
    canActivate: [FuncaoGuard],
  },
  {
    path: 'clientes',
    loadChildren: () =>
      import('./clientes/clientes.module').then((m) => m.ClientesModule),
    canActivate: [FuncaoGuard],
  },
  { path: '', pathMatch: 'full', redirectTo: 'pedidos' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
