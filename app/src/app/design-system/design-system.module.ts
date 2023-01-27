import { CommonModule, registerLocaleData } from '@angular/common';
import ptBr from '@angular/common/locales/pt';
import { DEFAULT_CURRENCY_CODE, LOCALE_ID, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTooltipModule } from '@angular/material/tooltip';
import { IConfig, NgxMaskModule } from 'ngx-mask';

import { ClientesRoutingModule } from './../clientes/clientes-routing.module';
import { PedidosRoutingModule } from './../pedidos/pedidos-routing.module';
import { AcompanhamentoDePedidoComponent } from './acompanhamento-de-pedido/acompanhamento-de-pedido.component';
import { AddPrimeiroItemComponent } from './add-primeiro-item/add-primeiro-item.component';
import { AddSegundoItemComponent } from './add-segundo-item/add-segundo-item.component';
import { CabecalhoComponent } from './cabecalho/cabecalho.component';
import { CaixaDePedidoComponent } from './caixa-de-pedido/caixa-de-pedido.component';
import { CaixaStatusCacambaComponent } from './caixa-status-cacamba/caixa-status-cacamba.component';
import { CaixaStatusComponent } from './caixa-status/caixa-status.component';
import { ClienteFormComponent } from './cliente-form/cliente-form.component';
import { ConectorHorizontalComponent } from './conector-horizontal/conector-horizontal.component';
import { ConectorVerticalComponent } from './conector-vertical/conector-vertical.component';
import { EnderecoFormComponent } from './endereco-form/endereco-form.component';
import { FiltrosComponent } from './filtros/filtros.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { NavComponent } from './nav/nav.component';
import { NotaFiscalComponent } from './nota-fiscal/nota-fiscal.component';
import { PopupConfirmacaoComponent } from './popup-confirmacao/popup-confirmacao.component';
import { PopupInformativoComponent } from './popup-informativo/popup-informativo.component';
import { ProcurarClienteComponent } from './procurar-cliente/procurar-cliente.component';
import { VerItemPedidoComponent } from './ver-item-pedido/ver-item-pedido.component';

registerLocaleData(ptBr)

const maskConfig: Partial<IConfig> = {
  validation: false,
};
@NgModule({
  declarations: [
    CaixaDePedidoComponent,
    CaixaStatusComponent,
    ConectorVerticalComponent,
    ConectorHorizontalComponent,
    NavComponent,
    AcompanhamentoDePedidoComponent,
    CaixaStatusCacambaComponent,
    NavMenuComponent,
    FiltrosComponent,
    PopupConfirmacaoComponent,
    CabecalhoComponent,
    EnderecoFormComponent,
    ClienteFormComponent,
    ProcurarClienteComponent,
    AddPrimeiroItemComponent,
    AddSegundoItemComponent,
    VerItemPedidoComponent,
    PopupInformativoComponent,
    NotaFiscalComponent,
  ],
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatSidenavModule,
    MatListModule,
    MatExpansionModule,
    PedidosRoutingModule,
    ClientesRoutingModule,
    MatMenuModule,
    MatDividerModule,
    MatFormFieldModule,
    MatInputModule,
    MatRadioModule,
    FormsModule,
    MatDialogModule,
    MatTooltipModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCheckboxModule,
    MatChipsModule,
    ReactiveFormsModule,
    NgxMaskModule.forRoot(maskConfig),
    MatAutocompleteModule,
    MatSelectModule,
    MatProgressSpinnerModule
  ],
  exports: [
    CaixaDePedidoComponent,
    CaixaStatusComponent,
    NavComponent,
    AcompanhamentoDePedidoComponent,
    ConectorHorizontalComponent,
    ConectorVerticalComponent,
    CaixaStatusCacambaComponent,
    NavMenuComponent,
    FiltrosComponent,
    PopupConfirmacaoComponent,
    CabecalhoComponent,
    EnderecoFormComponent,
    ClienteFormComponent,
    ProcurarClienteComponent,
    AddPrimeiroItemComponent,
    AddSegundoItemComponent,
    VerItemPedidoComponent,
    NotaFiscalComponent
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'pt-BR' },
    { provide: DEFAULT_CURRENCY_CODE, useValue: 'BRL' },
  ],
})
export class DesignSystemModule { }
