import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatRadioModule } from '@angular/material/radio';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { IConfig, NgxMaskModule } from 'ngx-mask';

import { DesignSystemModule } from '../design-system/design-system.module';
import { ClientesRoutingModule } from './clientes-routing.module';
import { ClientesComponent } from './pages/clientes/clientes.component';
import { EditarClienteComponent } from './pages/editar-cliente/editar-cliente.component';
import { EditarEnderecoComponent } from './pages/editar-endereco/editar-endereco.component';

const maskConfig: Partial<IConfig> = {
  validation: false,
};
@NgModule({
  declarations: [ClientesComponent, EditarClienteComponent, EditarEnderecoComponent],
  imports: [
    CommonModule,
    ClientesRoutingModule,
    DesignSystemModule,
    MatTableModule,
    MatIconModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatButtonModule,
    MatMenuModule,
    MatDialogModule,
    MatRadioModule,
    MatPaginatorModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatMomentDateModule,
    MatTooltipModule,
    NgxMaskModule.forRoot(maskConfig)
  ],
})
export class ClientesModule { }
