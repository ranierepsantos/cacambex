import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';

import { DesignSystemModule } from './../design-system/design-system.module';
import { IdentidadeAcessoRoutingModule } from './identidade-acesso-routing.module';
import { AutoCadastroComponent } from './pages/auto-cadastro/auto-cadastro.component';
import { ConfirmacaoEmailComponent } from './pages/confirmacao-email/confirmacao-email.component';
import { EditarUsuarioComponent } from './pages/editar-usuario/editar-usuario.component';
import { RedefinirSenhaComponent } from './pages/redefinir-senha/redefinir-senha.component';
import { ResetarSenhaComponent } from './pages/resetar-senha/resetar-senha.component';
import { SigninComponent } from './pages/signin/signin.component';
import { SolicitarNovaSenhaComponent } from './pages/solicitar-nova-senha/solicitar-nova-senha.component';
import { UsuariosComponent } from './pages/usuarios/usuarios.component';

@NgModule({
  declarations: [
    SigninComponent,
    SolicitarNovaSenhaComponent,
    ResetarSenhaComponent,
    RedefinirSenhaComponent,
    ConfirmacaoEmailComponent,
    AutoCadastroComponent,
    UsuariosComponent,
    EditarUsuarioComponent,
  ],
  imports: [
    CommonModule,
    IdentidadeAcessoRoutingModule,
    DesignSystemModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatIconModule,
    MatProgressBarModule,
    MatButtonModule,
    MatTableModule,
    MatMenuModule,
    MatPaginatorModule,
    MatSortModule,
    MatDialogModule,
    MatSlideToggleModule,
    MatRadioModule,
    MatTooltipModule,
    MatSnackBarModule,
    MatDatepickerModule,
    MatNativeDateModule,
    ReactiveFormsModule,
    MatMomentDateModule,
    MatProgressSpinnerModule,
    MatAutocompleteModule,
    MatCardModule
  ],
})
export class IdentidadeAcessoModule { }
