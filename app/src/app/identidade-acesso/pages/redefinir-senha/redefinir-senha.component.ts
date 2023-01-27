import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SnackResponseService } from 'src/app/design-system/snack-response.service';

import { AutorizacaoUsuario } from '../../interfaces/autorizacao-usuario';
import { TokenServico } from '../../servicos/token.servico';
import { AutorizacaoServico } from './../../servicos/autorizacao.service';

@Component({
  templateUrl: './redefinir-senha.component.html',
  styleUrls: ['./redefinir-senha.component.css']
})
export class RedefinirSenhaComponent implements OnInit {
  autorizacao = {} as AutorizacaoUsuario;
  enviando: boolean = false;
  hide = true;
  mudarSenhaForm: FormGroup = {} as FormGroup;
  token: string = ' ';

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private tokenServico: TokenServico,
    private autorizacaoServico: AutorizacaoServico,
    private snackBar: SnackResponseService) {
    this.token = this.tokenServico.token;
    this.tokenServico.usuario.subscribe((usuario) => {
      this.autorizacao.email = usuario.email;
    });
  }

  ngOnInit(): void {
    this.mudarSenhaForm = this.fb.group({
      novaSenha: ['', [Validators.required, Validators.minLength(6)]],
    })
  }
  proximo() {
    this.enviando = true;
    this.autorizacao.senha = this.mudarSenhaForm.get('novaSenha')?.value;
    this.autorizacaoServico.login(this.autorizacao).subscribe((resposta) => {
      this.enviando = false;
      this.snackBar.mostrarMensagem('Senha correta! Agora você pode alterar sua senha.');
      this.router.navigate(['identidade-acesso/resetar-senha', this.token]);
    }, (error) => {
      this.enviando = false;
      this.snackBar.mostrarMensagem(error.error.mensagem, true);
    });
  }
}
