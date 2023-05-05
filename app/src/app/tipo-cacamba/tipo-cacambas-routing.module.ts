import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TipoCacambaComponent } from './pages/tipo-cacamba/tipo-cacamba.component';
import { EditarTipoComponent } from './pages/editar-tipo/editar-tipo/editar-tipo.component';


const routes: Routes = [
  { path: '', component: TipoCacambaComponent },
  { path: 'editar', component: EditarTipoComponent },
  { path: '', pathMatch: 'full', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TipoCacambaRoutingModule { }
