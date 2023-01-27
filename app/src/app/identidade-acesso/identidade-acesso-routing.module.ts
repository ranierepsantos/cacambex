import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { FuncaoGuard } from './guarda/funcao.guard';
import { AutoCadastroComponent } from './pages/auto-cadastro/auto-cadastro.component';
import { ConfirmacaoEmailComponent } from './pages/confirmacao-email/confirmacao-email.component';
import { RedefinirSenhaComponent } from './pages/redefinir-senha/redefinir-senha.component';
import { ResetarSenhaComponent } from './pages/resetar-senha/resetar-senha.component';
import { SigninComponent } from './pages/signin/signin.component';
import { SolicitarNovaSenhaComponent } from './pages/solicitar-nova-senha/solicitar-nova-senha.component';
import { UsuariosComponent } from './pages/usuarios/usuarios.component';

const routes: Routes = [
  { path: '', component: SigninComponent },
  { path: 'solicitar-nova-senha', component: SolicitarNovaSenhaComponent },
  { path: 'resetar-senha/:token', component: ResetarSenhaComponent },
  { path: 'redefinir-senha', component: RedefinirSenhaComponent },
  { path: 'auto-cadastro', component: AutoCadastroComponent },
  { path: 'confirmacao-email', component: ConfirmacaoEmailComponent },
  { path: 'usuarios', component: UsuariosComponent, canActivate: [FuncaoGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class IdentidadeAcessoRoutingModule { }
