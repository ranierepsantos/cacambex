import { CommonModule, registerLocaleData } from '@angular/common';
import ptBr from '@angular/common/locales/pt';
import { DEFAULT_CURRENCY_CODE, LOCALE_ID, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { IConfig, NgxMaskModule } from 'ngx-mask';

import { DesignSystemModule } from './../design-system/design-system.module';
import { EditarPedidoComponent } from './pages/editar-pedido/editar-pedido.component';
import { EmitirCtrComponent } from './pages/emitir-ctr/emitir-ctr.component';
import { EnderecosComponent } from './pages/enderecos/enderecos.component';
import { GerenciarPedidoComponent } from './pages/gerenciar-pedido/gerenciar-pedido.component';
import { NovoPedidoComponent } from './pages/novo-pedido/novo-pedido.component';
import { SelecionarCacambaComponent } from './pages/selecionar-cacamba/selecionar-cacamba.component';
import { TelaPedidosComponent } from './pages/tela-pedidos/tela-pedidos.component';
import { PedidosRoutingModule } from './pedidos-routing.module';
import { ReciboComponent } from './pages/recibo/recibo.component';

const maskConfig: Partial<IConfig> = {
  validation: false,
};
registerLocaleData(ptBr)
@NgModule({
  declarations: [
    TelaPedidosComponent,
    NovoPedidoComponent,
    EnderecosComponent,
    SelecionarCacambaComponent,
    GerenciarPedidoComponent,
    EditarPedidoComponent,
    EmitirCtrComponent,
    ReciboComponent,
  ],
  imports: [
    CommonModule,
    PedidosRoutingModule,
    MatCardModule,
    DesignSystemModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatMenuModule,
    MatDialogModule,
    MatRadioModule,
    FormsModule,
    MatProgressSpinnerModule,
    MatTooltipModule,
    MatButtonToggleModule,
    MatGridListModule,
    MatStepperModule,
    ReactiveFormsModule,
    MatDividerModule,
    MatSelectModule,
    NgxMaskModule.forRoot(maskConfig)
  ],
  providers: [
    { provide: MatDialogRef, useValue: {} },
    { provide: LOCALE_ID, useValue: 'pt-BR' },
    { provide: DEFAULT_CURRENCY_CODE, useValue: 'BRL' },
  ]
})
export class PedidosModule { }
