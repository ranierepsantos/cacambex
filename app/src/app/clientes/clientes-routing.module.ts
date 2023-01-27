import { ClienteGuard } from './guardas/cliente.guard';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { EditarClienteComponent } from './pages/editar-cliente/editar-cliente.component';
import { ClientesComponent } from './pages/clientes/clientes.component';

const routes: Routes = [
  { path: '', component: ClientesComponent },
  { path: 'novo-cliente', component: EditarClienteComponent, resolve: { cliente: ClienteGuard } },
  { path: 'editar-cliente/:id', component: EditarClienteComponent, resolve: { cliente: ClienteGuard } },
  { path: '', pathMatch: 'full', redirectTo: '' },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ClientesRoutingModule { }
