import { CommonModule, registerLocaleData } from '@angular/common';
import ptBr from '@angular/common/locales/pt';
import { DEFAULT_CURRENCY_CODE, LOCALE_ID, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatRadioModule } from '@angular/material/radio';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { IConfig, NgxMaskModule } from 'ngx-mask';

import { DesignSystemModule } from '../design-system/design-system.module';
import { TipoCacambaRoutingModule } from './tipo-cacambas-routing.module';
import { TipoCacambaComponent } from './pages/tipo-cacamba/tipo-cacamba.component';
import { EditarTipoComponent } from './pages/editar-tipo/editar-tipo/editar-tipo.component';
//import { EditarCacambaComponent } from './pages/editar-cacamba/editar-cacamba.component';

registerLocaleData(ptBr)
const maskConfig: Partial<IConfig> = {
  validation: false,
};
@NgModule({
  declarations: [TipoCacambaComponent, EditarTipoComponent],
  imports: [
    CommonModule,
    FormsModule,
    TipoCacambaRoutingModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatRadioModule,
    MatDialogModule,
    MatButtonModule,
    MatTableModule,
    MatSortModule,
    MatMenuModule,
    MatPaginatorModule,
    DesignSystemModule,
    NgxMaskModule.forRoot(maskConfig),
    MatCardModule
  ],
  providers: [{ provide: LOCALE_ID, useValue: 'pt-BR' },
  { provide: DEFAULT_CURRENCY_CODE, useValue: 'BRL' },]
})
export class TipoCacambaModule { }
