import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SnackResponseService } from 'src/app/design-system/snack-response.service';

import { AutorizacaoUsuario } from '../../interfaces/autorizacao-usuario';
import { AutorizacaoServico } from '../../servicos/autorizacao.service';
import { TokenServico } from '../../servicos/token.servico';

@Component({
  templateUrl: "./signin.component.html",
  styleUrls: ["./signin.component.css"],
})
export class SigninComponent implements OnInit {
  acessando!: boolean;
  autorizacaoUsuario: AutorizacaoUsuario = {} as AutorizacaoUsuario;
  constructor(
    private router: Router,
    private autoServico: AutorizacaoServico,
    private tokenServico: TokenServico,
    private snackBar: SnackResponseService
  ) { }

  ngOnInit(): void { }

  login() {
    this.acessando = true;
    this.autoServico.login(this.autorizacaoUsuario).subscribe(
      (x: any) => {
        this.tokenServico.token = x.dados.token;
        this.acessando = false;
        this.router.navigateByUrl("/pedidos");
      }, (e: any) => {
        this.snackBar.mostrarMensagem(e.error.mensagem, true)
        this.snackBar.mostrarMensagem(e.message, true)
        this.acessando = false
      });
  }
}
