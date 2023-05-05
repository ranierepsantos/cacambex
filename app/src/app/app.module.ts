import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatGridListModule } from '@angular/material/grid-list';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DesignSystemModule } from './design-system/design-system.module';
import { MatPaginatorBr } from './extensoes/mat-paginator-br';
import { AutorizacaoGuarda } from './identidade-acesso/guarda/autorizacao.guarda';
import { InvalidTokenInterceptor } from './identidade-acesso/servicos/invalid-token.interceptor';
import { TokenInterceptor } from './identidade-acesso/servicos/token.interceptor';
import { PedidosModule } from './pedidos/pedidos.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    DesignSystemModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    PedidosModule,
    HttpClientModule,
    MatSnackBarModule,
    FormsModule,
    MatDatepickerModule,
    MatSelectModule,
    MatGridListModule,
  ],
  providers: [
    { provide: LocationStrategy, useClass: HashLocationStrategy },
    AutorizacaoGuarda,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: InvalidTokenInterceptor,
      multi: true,
    },
    { provide: MatPaginatorIntl, useClass: MatPaginatorBr },
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
